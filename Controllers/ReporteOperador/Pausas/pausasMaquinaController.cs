using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.PausaMaquina;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ReporteOperador.Pausas
{
    [Route("api/[controller]")]
    [ApiController]
    public class pausasMaquinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public pausasMaquinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<pausasMaquinaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PausasMaquinaDto>>> GetpausasMaquina()
        {
            var pausasMaquina = await _context.pausasMaquina.ToListAsync();
            var pausasMaquinaDto = _mapper.Map<List<PausasMaquinaDto>>(pausasMaquina);

            return Ok(pausasMaquinaDto);
        }
        

        // GET api/<pausasMaquinaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PausasMaquinaDto>> GetpausasMaquina(int id)
        {
            var pausasMaquina = await _context.pausasMaquina.FindAsync(id);
            var pausasMaquinaDto = _mapper.Map<PausasMaquinaDto>(pausasMaquina);
            if (pausasMaquinaDto == null)
            {
                return NotFound($"No se encontro la pausa con el id {id}");
            }
            return Ok(pausasMaquinaDto);
        }

        // GET api/<pausasMaquinaController>/5
        [HttpGet("get/maquina/{id}")]
        public async Task<ActionResult<PausasMaquinaDto>> GetpausasMaquinaActiva(int id)
        {
            var pausasMaquina = await _context.pausasMaquina
                .Where(pausa => pausa.maquina == id && pausa.fin == null)
                .FirstOrDefaultAsync();

            var pausasMaquinaDto = _mapper.Map<PausasMaquinaDto>(pausasMaquina);

            if (pausasMaquinaDto == null)
            {
                return NotFound($"No se encontro la pausa con el id {id}");
            }

            return Ok(pausasMaquinaDto);
        }

        // POST api/<pausasMaquinaController>
        [HttpPost("post")]
        public async Task<ActionResult<pausasMaquina>> PostpausasMaquina(AddPausasMaquinaDto addPausasMaquinaDto)
        {
            var pausasMaquina = _mapper.Map<pausasMaquina>(addPausasMaquinaDto);
            _context.pausasMaquina.Add(pausasMaquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetpausasMaquina", new { id = pausasMaquina.id }, pausasMaquina);
        }

        // PUT api/<pausasMaquinaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutpausasMaquina(int id, UpdatePausasMaquinaDto updatePausasMaquinaDto)
        {
            var pausasMaquina = await _context.pausasMaquina.FindAsync(id);

            if (pausasMaquina == null)
            {
                return NotFound($"No se encontro la pausa con el id {id}");
            }

            _mapper.Map(updatePausasMaquinaDto, pausasMaquina);
            _context.Entry(pausasMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pausasMaquinaExists(id))
                {
                    return NotFound($"No se encontro la pausa con el id {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updatePausasMaquinaDto);
        }

        // DELETE api/<pausasMaquinaController>/5
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeletepausasMaquina(int id)
        {
            var pausasMaquina = await _context.pausasMaquina.FindAsync(id);
            if (pausasMaquina == null)
            {
                return NotFound($"No se encontro la pausa con el id {id}");
            }
            _context.pausasMaquina.Remove(pausasMaquina);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool pausasMaquinaExists(int id)
        {
            return _context.pausasMaquina.Any(e => e.id == id);
        }
    }
}
