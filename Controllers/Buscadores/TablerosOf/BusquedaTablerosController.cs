using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Buscadores.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaTablerosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        [HttpGet("get/buscarGlobal/{of}")]
        public async Task<IActionResult> BuscarGlobalPorOF(int of)
        {
            // Buscar en procesoOf
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                    .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
            .Include(f => f.oFNavigation)
                .FirstOrDefaultAsync(u => u.oF == of);

            // Buscar en tarjetaOf
            var tarjetaOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .FirstOrDefaultAsync(u => u.oF == of);

            // Mapear los DTOs
            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);
            var tarjetaOfDto = _mapper.Map<TarjetaOfDto>(tarjetaOf);

            // Si no se encuentra nada, retornar NotFound
            if (procesoOfDto == null && tarjetaOfDto == null)
            {
                return NotFound($"No se encontró ninguna información para la OF: {of}");
            }

            // Devolver ambos resultados en un solo JSON
            return Ok(new
            {
                ProcesoOf = procesoOfDto,
                TarjetaOf = tarjetaOfDto
            });
        }

    }
}
