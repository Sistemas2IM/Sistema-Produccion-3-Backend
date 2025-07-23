using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogCambiosProceso;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.logCambiosOf;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class logCambiosOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public logCambiosOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<logCambiosOfController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<logCambiosOfDto>>> GetlogCambiosOf()
        {
            var logCambiosOf = await _context.logCambiosOf.ToListAsync();
            var logCambiosOfDto = _mapper.Map<List<logCambiosOfDto>>(logCambiosOf);

            return Ok(logCambiosOfDto);
        }

        // GET api/<logCambiosOfController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<logCambiosOfDto>>> GetlogCambiosOfId(int id)
        {
            var logCambiosOf = await _context.logCambiosOf
                .Where(x => x.log_id == id)
                .ToListAsync();

            var logCambiosOfDto = _mapper.Map<List<logCambiosOfDto>>(logCambiosOf);

            return Ok(logCambiosOfDto);
        }

        // GET: API/logCambiosOfController
        [HttpGet("get/oF/{id}")]
        public async Task<ActionResult<IEnumerable<logCambiosOfDto>>> GetlogCambiosOfByOF(int id)
        {
            var logCambiosOf = await _context.logCambiosOf
                .Where(x => x.oF == id)
                .ToListAsync();

            var logCambiosOfDto = _mapper.Map<List<logCambiosOfDto>>(logCambiosOf);

            return Ok(logCambiosOfDto);
        }

        // POST api/<logCambiosOfController>
        [HttpPost("post")]
        public async Task<ActionResult<logCambiosOfDto>> PostlogCambiosOf(AddlogCambiosOfDto addlogCambiosOfDto)
        {
            var logCambiosOf = _mapper.Map<logCambiosOf>(addlogCambiosOfDto);
            _context.logCambiosOf.Add(logCambiosOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetlogCambiosOf", new { id = logCambiosOf.log_id }, logCambiosOf);
        }

        // PUT api/<logCambiosOfController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutlogCambiosOf(int id, UpdatelogCambiosOfDto updatelogCambiosOfDto)
        {
            var logCambiosOf = await _context.logCambiosOf.FindAsync(id);

            if (logCambiosOf == null)
            {
                return NotFound($"No se encontro el log de cambios de of con el ID: {id}");
            }

            _mapper.Map(updatelogCambiosOfDto, logCambiosOf);
            _context.Entry(logCambiosOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!logCambiosOfExists(id))
                {
                    return NotFound($"No se encontro el log de cambios de of con el ID: {id}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool logCambiosOfExists(int id)
        {
            return _context.logCambiosOf.Any(e => e.log_id == id);
        }
    }
}
