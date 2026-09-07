using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.HistorialVencimientoOf;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf.HistorialVencimientoOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialVencimientoOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public HistorialVencimientoOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<HistorialVencimientoOfController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<HistorialVencimientoOfDto>>> GetHistorial()
        {
            var historial = await _context.historialVencimientoOf
                .Include(u => u.registradoPorNavigation)
                .Include(u => u.motivoCambioNavigation)
                .ToListAsync();

            var historialDto = _mapper.Map<List<HistorialVencimientoOfDto>>(historial);

            return Ok(historialDto);
        }

        // GET api/<historialVencimientoOfController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<HistorialVencimientoOfDto>> GetHistorialById(int id)
        {
            var historial = await _context.historialVencimientoOf
                .Include(u => u.registradoPorNavigation)
                .Include(u => u.motivoCambioNavigation)
                .FirstOrDefaultAsync(h => h.idHistorial == id);

            var historialDto = _mapper.Map<HistorialVencimientoOfDto>(historial);

            if (historialDto == null)
            {
                return NotFound();
            }

            return Ok(historialDto);
        }

        // GET por oF
        [HttpGet("get/of/{oF}")]
        public async Task<ActionResult<IEnumerable<HistorialVencimientoOfDto>>> GetHistorialByOF(int oF)
        {
            var historial = await _context.historialVencimientoOf
                .Include(u => u.registradoPorNavigation)
                .Include(u => u.motivoCambioNavigation)
                .Where(h => h.oF == oF)
                .ToListAsync();
            var historialDto = _mapper.Map<List<HistorialVencimientoOfDto>>(historial);

            if (historialDto == null || historialDto.Count == 0)
            {
                return NotFound();
            }

            return Ok(historialDto);
        }

        // POST api/<historialVencimientoOfController>
        [HttpPost("post")]
        public async Task<ActionResult<HistorialVencimientoOfDto>> PostHistorial(HistorialVencimientoOfDto historialDto)
        {
            var historial = _mapper.Map<historialVencimientoOf>(historialDto);
            _context.historialVencimientoOf.Add(historial);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistorialById), new { id = historial.idHistorial }, historialDto);
        }

    }
}
