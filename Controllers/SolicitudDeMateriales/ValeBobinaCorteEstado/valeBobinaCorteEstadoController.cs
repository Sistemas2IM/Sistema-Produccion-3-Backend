using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina.ValeBobinaCorteEstado;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales.ValeBobinaCorteEstado
{
    [Route("api/[controller]")]
    [ApiController]
    public class valeBobinaCorteEstadoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly base_nuevaContext _context;

        public valeBobinaCorteEstadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<valeBobinaCorteEstadoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ValeBobinaCorteEstadoDto>>> GetaleBobinaCorteEstado()
        {
            var corteEstados = await _context.valeBobinaCorteEstado.ToListAsync();
            var corteEstadosDto = _mapper.Map<List<ValeBobinaCorteEstadoDto>>(corteEstados);

            return Ok(corteEstadosDto);
        }

        // GET api/<valeBobinaCorteEstadoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ValeBobinaCorteEstadoDto>> GetaleBobinaCorteEstado(int id)
        {
            var corteEstado = await _context.valeBobinaCorteEstado.FindAsync(id);
            if (corteEstado == null)
            {
                return NotFound();
            }
            var corteEstadoDto = _mapper.Map<ValeBobinaCorteEstadoDto>(corteEstado);
            return Ok(corteEstadoDto);
        }

        // POST api/<valeBobinaCorteEstadoController>
        [HttpPost("post")]
        public async Task<ActionResult<valeBobina>> PostValeBobinaCorteEstado(AddValeBobinaCorteEstadoDto addCorteEstadoDto)
        {
            var corteEstado = _mapper.Map<valeBobinaCorteEstado>(addCorteEstadoDto);
            _context.valeBobinaCorteEstado.Add(corteEstado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetaleBobinaCorteEstado), new { id = corteEstado.idCorteEstado }, corteEstado);
        }

        // PUT api/<valeBobinaCorteEstadoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutValeBobinaCorteEstado(int id, UpdateValeBobinaCorteEstadoDto updateValeBobinaCorteEstadoDto)
        {
            var corteEstado = await _context.valeBobinaCorteEstado.FindAsync(id);
            if (corteEstado == null)
            {
                return NotFound();
            }

            _mapper.Map(updateValeBobinaCorteEstadoDto, corteEstado);
            _context.Entry(corteEstado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValeBobinaCorteEstadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ValeBobinaCorteEstadoExists(int id)
        {
            return _context.valeBobinaCorteEstado.Any(e => e.idCorteEstado == id);
        }
    }
}
