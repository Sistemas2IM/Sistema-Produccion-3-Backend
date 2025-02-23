using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablerosGestionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public TablerosGestionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf FLEXOGRAFIA
        [HttpGet("get/flexo")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOfFlexo()
        {
            var procesoOf = await _context.procesoOf
                .Where(f => f.oFNavigation.lineaDeNegocio == "FLEXOGRAFIA")
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }
    }
}
