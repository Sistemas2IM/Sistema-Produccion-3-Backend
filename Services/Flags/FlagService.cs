using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Services.Flags
{
    public class FlagService : IFlagService
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public FlagService(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // --------------------------------------------------------------------------------
        // 1. OBTENER ACTIVAS ASYNC
        // --------------------------------------------------------------------------------
        public async Task<Dictionary<int, List<FlagDocumentoDto>>> ObtenerActivasAsync(int tipoDocumento, IReadOnlyCollection<int> idsDocumento)
        {
            if (idsDocumento == null || !idsDocumento.Any())
            {
                return new Dictionary<int, List<FlagDocumentoDto>>();
            }

            var flagsEntidades = await _context.flagDocumento
                .Include(f => f.idFlagNavigation)
                .Where(f => f.tipoDocumento == tipoDocumento &&
                            idsDocumento.Contains(f.idDocumento) &&
                            f.activa == true)
                .ToListAsync();

            if (!flagsEntidades.Any())
            {
                return new Dictionary<int, List<FlagDocumentoDto>>();
            }

            var diccionarioResult = flagsEntidades
                .GroupBy(f => f.idDocumento)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo
                        .OrderBy(f => ObtenerPesoAccion(f.idFlagNavigation?.efecto ?? f.idFlagNavigation?.codigo))
                        .ThenBy(f => f.idFlagNavigation?.nombre)
                        .Select(f => _mapper.Map<FlagDocumentoDto>(f))
                        .ToList()
                );

            return diccionarioResult;
        }

        // --------------------------------------------------------------------------------
        // 2. ASIGNAR MANUAL ASYNC (Implementación de IFlagService)
        // --------------------------------------------------------------------------------
        public async Task<FlagDocumentoDto> AsignarManualAsync(AddFlagDocumentoDto dto)
        {
            if (dto.idFlag == null || dto.tipoDocumento == null || dto.idDocumento == null)
            {
                throw new FlagValidationException("Los campos idFlag, tipoDocumento e idDocumento son obligatorios.");
            }

            int idFlag = dto.idFlag.Value;
            int tipoDocumento = dto.tipoDocumento.Value;
            int idDocumento = dto.idDocumento.Value;
            string? texto = string.IsNullOrWhiteSpace(dto.texto) ? null : dto.texto.Trim();

            // 1. La flag existe, está activa y su modo de asignación es MANUAL
            var flagCatalogo = await _context.catalogoFlag
                .FirstOrDefaultAsync(f => f.idFlag == idFlag);

            if (flagCatalogo == null || !flagCatalogo.activo)
            {
                throw new FlagValidationException($"La flag con ID {idFlag} no existe o no está activa.");
            }

            if (!string.Equals(flagCatalogo.modoAsignacion?.Trim(), "MANUAL", StringComparison.OrdinalIgnoreCase))
            {
                throw new FlagValidationException($"La flag '{flagCatalogo.nombre}' no es de asignación manual.");
            }

            // 2. La flag aplica al tipo de documento
            bool flagPermitida = await _context.flagTipoDocumento
                .AnyAsync(ftd => ftd.idFlag == idFlag && ftd.tipoDocumento == tipoDocumento);

            if (!flagPermitida)
            {
                throw new FlagValidationException($"La flag '{flagCatalogo.nombre}' no aplica al tipo de documento {tipoDocumento}.");
            }

            // 3. Tipo de documento soportado en esta etapa
            bool esOf = tipoDocumento == (int)TiposDocumento.TarjetaOf;
            bool esProceso = tipoDocumento == (int)TiposDocumento.ProcesoOf;

            if (!esOf && !esProceso)
            {
                throw new FlagValidationException($"El tipo de documento {tipoDocumento} aún no está soportado para asignación manual.");
            }

            // 4 y 5. El documento existe y no está cancelado ni archivado
            if (esOf)
            {
                var of = await _context.tarjetaOf
                    .AsNoTracking()
                    .Where(t => t.oF == idDocumento)
                    .Select(t => new { t.archivada, t.cancelada })
                    .FirstOrDefaultAsync();

                if (of == null)
                    throw new FlagNotFoundException($"La Orden de Fabricación {idDocumento} no existe.");

                if (of.cancelada == true || of.archivada == true)
                    throw new FlagValidationException($"La Orden de Fabricación {idDocumento} está cancelada o archivada.");
            }
            else
            {
                var proceso = await _context.procesoOf
                    .AsNoTracking()
                    .Where(p => p.idProceso == idDocumento)
                    .Select(p => new { p.archivada, p.cancelada, p.idTablero })
                    .FirstOrDefaultAsync();

                if (proceso == null)
                    throw new FlagNotFoundException($"El proceso {idDocumento} no existe.");

                if (proceso.cancelada == true || proceso.archivada == true)
                    throw new FlagValidationException($"El proceso {idDocumento} está cancelado o archivado.");

                // 6. La flag está configurada para el tablero del proceso
                bool flagEnTablero = proceso.idTablero != null && await _context.tableroFlag
                    .AnyAsync(tf => tf.idTablero == proceso.idTablero && tf.idFlag == idFlag);

                if (!flagEnTablero)
                    throw new FlagValidationException($"La flag '{flagCatalogo.nombre}' no está configurada para el tablero del proceso {idDocumento}.");
            }

            // 7. Texto obligatorio si la flag lo requiere
            if (flagCatalogo.requiereTexto && texto == null)
            {
                throw new FlagValidationException($"La flag '{flagCatalogo.nombre}' requiere un texto.");
            }

            // Usuario que registra (FK a usuario)
            string? registradoPor = string.IsNullOrWhiteSpace(dto.registradoPor) ? null : dto.registradoPor.Trim();
            if (registradoPor != null && !await _context.usuario.AnyAsync(u => u.user == registradoPor))
            {
                throw new FlagValidationException($"El usuario '{registradoPor}' no existe.");
            }

            // 8. No existe la misma flag activa en el documento
            bool yaExisteActiva = await _context.flagDocumento
                .AnyAsync(f => f.tipoDocumento == tipoDocumento &&
                               f.idDocumento == idDocumento &&
                               f.idFlag == idFlag &&
                               f.activa);

            if (yaExisteActiva)
            {
                throw new FlagConflictException($"La flag '{flagCatalogo.nombre}' ya está activa en este documento.");
            }

            // Creación: siempre una fila nueva, campos de propagación controlados por el servidor
            var nuevoRegistro = new flagDocumento
            {
                tipoDocumento = tipoDocumento,
                idDocumento = idDocumento,
                idFlag = idFlag,
                texto = texto,
                registradoPor = registradoPor,
                fechaRegistro = dto.fechaRegistro ?? DateTime.Now,
                activa = true,
                idFlagOrigen = null,
                idFlagRaiz = null,
                eventoRegistro = null
            };

            _context.flagDocumento.Add(nuevoRegistro);

            // Colisión concurrente contra el índice único flagDocumento_activaUnica_UX
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                throw new FlagConflictException($"La flag '{flagCatalogo.nombre}' ya está activa en este documento.");
            }

            nuevoRegistro.idFlagNavigation = flagCatalogo;
            return _mapper.Map<FlagDocumentoDto>(nuevoRegistro);
        }

        // --------------------------------------------------------------------------------
        // 3. AUXILIARES
        // --------------------------------------------------------------------------------
        private int ObtenerPesoAccion(string accion)
        {
            if (string.IsNullOrWhiteSpace(accion)) return 99;

            return accion.Trim().ToUpper() switch
            {
                "BLOQUEA" => 1,
                "ADVIERTE" => 2,
                "INFORMA" => 3,
                _ => 4
            };
        }
    }
}
