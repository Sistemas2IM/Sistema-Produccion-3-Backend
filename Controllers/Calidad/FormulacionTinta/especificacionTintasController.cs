using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta.EspecificacionTintas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FormulacionTinta
{
    [Route("api/[controller]")]
    [ApiController]
    public class especificacionTintasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public especificacionTintasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<especificacionTintasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EspecificacionTintasDto>>> GetEspecificacion()
        {
            var especificacion = await _context.especificacionTintas.ToListAsync();
            var especificacionDto = _mapper.Map<List<EspecificacionTintasDto>>(especificacion);
            return Ok(especificacionDto);
        }

        // GET api/<especificacionTintasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EspecificacionTintasDto>> GetEspecificacion(int id)
        {
            var especificacion = await _context.especificacionTintas.FirstOrDefaultAsync(e => e.idEspecificacion == id);
            if (especificacion == null)

            {
                return NotFound();
            }

            var especificacionDto = _mapper.Map<EspecificacionTintasDto>(especificacion);

            return Ok(especificacionDto);
        }

        // POST api/<especificacionTintasController>
        [HttpPost("post")]
        public async Task<ActionResult<especificacionTintas>> PostEspecificaciones(AddEspecificacionTintasDto addEspecificacionTintasDto)
        {
            var especificacion = _mapper.Map<especificacionTintas>(addEspecificacionTintasDto);

            _context.especificacionTintas.Add(especificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEspecificacion), new { id = especificacion.idEspecificacion }, especificacion);
        }

        // PUT api/<especificacionTintasController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutEspecificacion(int id, UpdateEspecificacionTintasDto updateEspecificacionTintasDto)
        {
            var especificacion = await _context.especificacionTintas.FindAsync(id);
            if (especificacion == null)
            {
                return NotFound();
            }

            _mapper.Map(updateEspecificacionTintasDto, especificacion);
            _context.Entry(especificacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!especificacionTintasExists(id))
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

        private bool especificacionTintasExists(int id)
        {
            return _context.especificacionTintas.Any(e => e.idEspecificacion == id);
        }

    }
}
