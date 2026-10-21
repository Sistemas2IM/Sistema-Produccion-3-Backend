using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento;
using Sistema_Produccion_3_Backend.DTO.Flags.FlagManual;
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
        public async Task<FlagDocumentoDto> AsignarManualAsync(CrearFlagManualDto dto)
        {
            // A. Validar tipo de documento soportado
            bool esOf = dto.tipoDocumento == (int)TiposDocumento.TarjetaOf;
            bool esProceso = dto.tipoDocumento == (int)TiposDocumento.ProcesoOf;

            if (!esOf && !esProceso)
            {
                throw new InvalidOperationException($"El tipo de documento {dto.tipoDocumento} no admite asignación manual en esta etapa.");
            }

            // B. Validar existencia del documento (OF o Proceso)
            if (esOf)
            {
                bool existeOf = await _context.tarjetaOf.AnyAsync(t => t.oF == dto.idDocumento && t.archivada != true);
                if (!existeOf)
                    throw new KeyNotFoundException($"La Orden de Fabricación (OF) {dto.idDocumento} no existe o está archivada.");
            }
            else if (esProceso)
            {
                bool existeProceso = await _context.procesoOf.AnyAsync(p => p.idProceso == dto.idDocumento && p.archivada != true);
                if (!existeProceso)
                    throw new KeyNotFoundException($"El proceso {dto.idDocumento} no existe o está archivado.");
            }

            // C. Validar catálogo: Activa y en modo MANUAL
            var flagCatalogo = await _context.catalogoFlag
                .FirstOrDefaultAsync(f => f.idFlag == dto.idFlag);

            if (flagCatalogo == null || !flagCatalogo.activo)
            {
                throw new KeyNotFoundException($"La flag con ID {dto.idFlag} no existe o no está activa.");
            }

            if (!string.Equals(flagCatalogo.modoAsignacion, "MANUAL", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Solo se permite asignar manualmente flags configuradas con modo 'MANUAL'.");
            }

            // D. Validar si la flag está habilitada para este tipo de documento
            bool flagPermitida = await _context.flagTipoDocumento
                .AnyAsync(ftd => ftd.idFlag == dto.idFlag && ftd.tipoDocumento == dto.tipoDocumento);

            if (!flagPermitida)
            {
                throw new InvalidOperationException($"La flag '{flagCatalogo.nombre}' no está autorizada para este tipo de documento.");
            }

            // E. Validar si ya existe activa (Previene duplicados)
            bool yaExisteActiva = await _context.flagDocumento
                .AnyAsync(f => f.tipoDocumento == dto.tipoDocumento &&
                               f.idDocumento == dto.idDocumento &&
                               f.idFlag == dto.idFlag &&
                               f.activa == true);

            if (yaExisteActiva)
            {
                throw new InvalidOperationException("La flag ya se encuentra asignada y activa en este documento.");
            }

            // F. Creación controlada por el servidor
            var nuevoRegistro = new flagDocumento
            {
                tipoDocumento = dto.tipoDocumento,
                idDocumento = dto.idDocumento,
                idFlag = dto.idFlag,
                texto = dto.texto?.Trim(),

                // 🚀 CORRECCIÓN: mapeo a 'registradoPor' como string
                registradoPor = dto.idUsuario?.ToString(),

                fechaRegistro = DateTime.Now,
                activa = true,
                idFlagOrigen = null,
                idFlagRaiz = null,
                eventoRegistro = null
            };

            _context.flagDocumento.Add(nuevoRegistro);

            // G. Guardar y atrapar colisión concurrente (código 2601 / 2627)
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    throw new InvalidOperationException("Conflicto de concurrencia: el documento ya tiene esta bandera activa.");
                }
                throw;
            }

            // H. Retornar DTO de respuesta
            return new FlagDocumentoDto
            {
                idFlagDocumento = nuevoRegistro.idFlagDocumento,
                idFlag = nuevoRegistro.idFlag,
                tipoDocumento = nuevoRegistro.tipoDocumento,
                idDocumento = nuevoRegistro.idDocumento,
                texto = nuevoRegistro.texto,
                activa = nuevoRegistro.activa,
                idFlagOrigen = nuevoRegistro.idFlagOrigen,
                idFlagRaiz = nuevoRegistro.idFlagRaiz,
                eventoRegistro = nuevoRegistro.eventoRegistro,
                registradoPor = nuevoRegistro.registradoPor,
                fechaRegistro = nuevoRegistro.fechaRegistro
            };
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
