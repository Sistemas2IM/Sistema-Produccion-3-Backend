using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.DecisionConciliacion;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales.Conciliacion.DecisionConciliacion
{
    [Route("api/[controller]")]
    [ApiController]
    public class decisionConciliacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public decisionConciliacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<decisionConciliacionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DecisionConciliacionDto>>> GetDecisionesConciliacion()
        {
            var decisionesConciliacion = await _context.decisionConciliacion
                .ToListAsync();

            var decisionesConciliacionDto = _mapper.Map<List<DecisionConciliacionDto>>(decisionesConciliacion);

            return Ok(decisionesConciliacionDto);
        }

        // GET api/<decisionConciliacionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DecisionConciliacionDto>> GetDecisionConciliacion(int id)
        {
            var decisionConciliacion = await _context.decisionConciliacion
                .FirstOrDefaultAsync(d => d.idDecision == id);

            if (decisionConciliacion == null)
            {
                return NotFound();
            }

            var decisionConciliacionDto = _mapper.Map<DecisionConciliacionDto>(decisionConciliacion);

            return Ok(decisionConciliacionDto);
        }
    }
}
