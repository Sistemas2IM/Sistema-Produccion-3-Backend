using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.EstadoReporte;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.EstadoReporte
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadosReporteController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public estadosReporteController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<estadosReporteController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EstadoReporteDto>>> GetestadosReporte()
        {
            var estadoReporte = await _context.estadosReporte
                .Include(e => e.tipoReporteNavigation)
                .ToListAsync();
            var estadoReporteDto = _mapper.Map<List<EstadoReporteDto>>(estadoReporte);
            return Ok(estadoReporteDto);
        }

        // GET api/<estadosReporteController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EstadoReporteDto>> GetestadoReporte(int id)
        {
            var estadoReporte = await _context.estadosReporte
                .Include(e => e.tipoReporteNavigation)
                .FirstOrDefaultAsync(e => e.idEstadoReporte == id);
            if (estadoReporte == null)
            {
                return NotFound();
            }
            var estadoReporteDto = _mapper.Map<EstadoReporteDto>(estadoReporte);
            return Ok(estadoReporteDto);
        }

        // get por tipoReporte
        [HttpGet("get/tipoReporte/{idTipo}")]
        public async Task<ActionResult<IEnumerable<EstadoReporteDto>>> GetestadosReporteByTipo(int idTipo)
        {
            var estadoReporte = await _context.estadosReporte
                .Include(e => e.tipoReporteNavigation)
                .Where(e => e.tipoReporteNavigation.idTipoReporte == idTipo)
                .ToListAsync();
            var estadoReporteDto = _mapper.Map<List<EstadoReporteDto>>(estadoReporte);
            return Ok(estadoReporteDto);
        }

    }
}
