using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso.BitacoraEvidencia;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.BitacoraCaso.BitacoraEvidencia
{
    [Route("api/[controller]")]
    [ApiController]
    public class bitacoraEvidenciaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public bitacoraEvidenciaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("post/batch")]
        public async Task<IActionResult> GuardarRelaciones([FromBody] List<RelacionBitacoraAnexoDto> batch)
        {
            // 1. Obtenemos listas únicas de IDs para no traer datos repetidos
            var bitacorasIds = batch.Select(x => x.idBitacora).Distinct().ToList();
            var anexosIds = batch.Select(x => x.idAnexo).Distinct().ToList();

            // 2. Traemos todas las bitácoras involucradas (incluyendo sus anexos actuales)
            var bitacoras = await _context.bitacoraCaso
                .Include(b => b.idAnexo) // Tu colección directa
                .Where(b => bitacorasIds.Contains(b.idEvento))
                .ToListAsync();

            // 3. Traemos todos los anexos involucrados
            // (Asumo que la llave primaria de anexos_NEXO se llama 'Id' o 'idAnexo')
            var anexos = await _context.anexos_NEXO
                .Where(a => anexosIds.Contains(a.Id))
                .ToListAsync();

            // 4. Armamos las relaciones en memoria
            foreach (var item in batch)
            {
                var bitacora = bitacoras.FirstOrDefault(b => b.idEvento == item.idBitacora);
                var anexo = anexos.FirstOrDefault(a => a.Id == item.idAnexo);

                if (bitacora != null && anexo != null)
                {
                    // Evitamos insertar un duplicado que rompa tu Llave Primaria compuesta
                    if (!bitacora.idAnexo.Any(a => a.Id == anexo.Id))
                    {
                        bitacora.idAnexo.Add(anexo);
                    }
                }
            }

            // 5. EF Core descubre qué relaciones faltan y hace los INSERTs en bitacoraEvidencia automáticamente
            await _context.SaveChangesAsync();

            return Ok("Relaciones guardadas correctamente");
        }
    }
}
