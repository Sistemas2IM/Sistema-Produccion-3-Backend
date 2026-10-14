using AutoMapper;
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

        public async Task<Dictionary<int, List<FlagDocumentoDto>>> ObtenerActivasAsync(int tipoDocumento, IReadOnlyCollection<int> idsDocumento)
        {
            // 1. Validación temprana: si no hay IDs, devolvemos un diccionario vacío inmediatamente
            if (idsDocumento == null || !idsDocumento.Any())
            {
                return new Dictionary<int, List<FlagDocumentoDto>>();
            }

            // 2. Consulta Única (Single Query) a la base de datos
            // Usamos Contains() para que EF Core genere un "WHERE idDocumento IN (1, 2, 3...)"
            var flagsEntidades = await _context.flagDocumento
                .Include(f => f.idFlagNavigation) // Traemos la relación con catalogoFlag
                .Where(f => f.tipoDocumento == tipoDocumento &&
                            idsDocumento.Contains(f.idDocumento) &&
                            f.activa == true) // Nota: Cambia a 'f.activa == 1' si EF lo mapeó como byte/int en lugar de bool
                .ToListAsync();

            // Si no encontró ninguna bandera, retornamos vacío
            if (!flagsEntidades.Any())
            {
                return new Dictionary<int, List<FlagDocumentoDto>>();
            }

            // 3. Agrupación, Ordenamiento y Mapeo en memoria
            var diccionarioResult = flagsEntidades
                .GroupBy(f => f.idDocumento)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo
                        // Orden 1: Por la acción (BLOQUEA, ADVIERTE, INFORMA)
                        .OrderBy(f => ObtenerPesoAccion(f.idFlagNavigation?.codigo))
                        // Orden 2: Por el nombre del flag alfabéticamente
                        .ThenBy(f => f.idFlagNavigation?.nombre)
                        // Mapeamos a DTO
                        .Select(f => _mapper.Map<FlagDocumentoDto>(f))
                        .ToList()
                );

            return diccionarioResult;
        }

        // 🚀 Método auxiliar para asignar prioridad al ordenamiento
        private int ObtenerPesoAccion(string accion)
        {
            if (string.IsNullOrWhiteSpace(accion)) return 99;

            // Asignamos un número según la urgencia/jerarquía
            return accion.Trim().ToUpper() switch
            {
                "BLOQUEA" => 1,
                "ADVIERTE" => 2,
                "INFORMA" => 3,
                _ => 4 // Cualquier otro estado desconocido se va al final
            };
        }
    }
}
