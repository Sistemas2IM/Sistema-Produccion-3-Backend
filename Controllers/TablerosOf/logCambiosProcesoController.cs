using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogCambiosProceso;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class logCambiosProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public logCambiosProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/logCambiosProcesoController
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<LogCambiosProcesoDto>>> GetlogCambiosProceso()
        {
            var logCambiosProceso = await _context.logCambiosProceso.ToListAsync();
            var logCambiosProcesoDto = _mapper.Map<List<LogCambiosProcesoDto>>(logCambiosProceso);

            return Ok(logCambiosProcesoDto);
        }

        // GET api/logCambiosProcesoController/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<LogCambiosProcesoDto>> GetlogCambiosProceso(int id)
        {
            var logCambiosProceso = await _context.logCambiosProceso.FindAsync(id);
            var logCambiosProcesoDto = _mapper.Map<LogCambiosProcesoDto>(logCambiosProceso);

            if (logCambiosProcesoDto == null)
            {
                return NotFound("No se encontro el log de cambios de proceso con ID: " + id);
            }

            return Ok(logCambiosProcesoDto);
        }

        // GET: api/logCambiosProcesoController
        [HttpGet("get/idProceso/{id}")]
        public async Task<ActionResult<IEnumerable<LogCambiosProcesoDto>>> GetlogCambiosProcesoIdProceso(int id)
        {
            var logCambiosProceso = await _context.logCambiosProceso
                .Where(x => x.proceso_id == id)
                .ToListAsync();

            var logCambiosProcesoDto = _mapper.Map<List<LogCambiosProcesoDto>>(logCambiosProceso);

            return Ok(logCambiosProcesoDto);
        }

        // POST api/logCambiosProcesoController
        [HttpPost("post")]
        public async Task<ActionResult<LogCambiosProcesoDto>> PostlogCambiosProceso(AddLogCambiosProcesoDto createLogCambiosProceso)
        {
            var logCambiosProceso = _mapper.Map<logCambiosProceso>(createLogCambiosProceso);
            _context.logCambiosProceso.Add(logCambiosProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetlogCambiosProceso", new { id = logCambiosProceso.log_id }, logCambiosProceso);
        }

        // PUT api/logCambiosProcesoController/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutlogCambiosProceso(int id, UpdateLogCambiosProcesoDto updateLogCambiosProceso)
        {
            var logCambiosProceso = await _context.logCambiosProceso.FindAsync(id);

            if (logCambiosProceso == null)
            {
                return NotFound("No se encontro el log de cambios de proceso con el ID: " + id);
            }

            _mapper.Map(updateLogCambiosProceso, logCambiosProceso);
            _context.Entry(logCambiosProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!logCambiosProcesoExists(id))
                {
                    return NotFound("No se encontro el log de cambios de proceso con el ID: " + id);
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool logCambiosProcesoExists(int id)
        {
            return _context.logCambiosProceso.Any(e => e.log_id == id);
        }
    }
}
