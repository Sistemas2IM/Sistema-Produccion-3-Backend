using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class logProgramacionDetalleController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public logProgramacionDetalleController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<logProgramacionDetalleController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<LogProgramacionDetalleDto>>> GetLogProgramacionDetalle()
        {
            var logProgramacionDetalles = await _context.logProgramacionDetalle.ToListAsync();
            var logProgramacionDetallesDto = _mapper.Map<List<LogProgramacionDetalleDto>>(logProgramacionDetalles);

            return Ok(logProgramacionDetallesDto);
        }

        // GET api/<logProgramacionDetalleController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<LogProgramacionDetalleDto>> GetLogProgramacionDetalle(int id)
        {
            var logProgramacionDetalle = await _context.logProgramacionDetalle.FindAsync(id);

            if (logProgramacionDetalle == null)
            {
                return NotFound();
            }
            var logProgramacionDetalleDto = _mapper.Map<LogProgramacionDetalleDto>(logProgramacionDetalle);

            return Ok(logProgramacionDetalleDto);
        }

        // POST api/<logProgramacionDetalleController>
        [HttpPost("post")]
        public async Task<ActionResult<logProgramacionDetalle>> PostProgramacionDetalle(AddLogProgramacionDetalleDto addLogProgramacionDetalleDto)
        {
            var logProgramacionDetalle = _mapper.Map<logProgramacionDetalle>(addLogProgramacionDetalleDto);
            _context.logProgramacionDetalle.Add(logProgramacionDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogProgramacionDetalle), new { id = logProgramacionDetalle.idLogProgramacionDetalle }, logProgramacionDetalle);
        }

        // PUT api/<logProgramacionDetalleController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutLogProgramacionDetalle(int id, UpdateLogProgramacionDetalleDto updateLogProgramacionDetalleDto)
        {
            
            var logProgramacionDetalle = await _context.logProgramacionDetalle.FindAsync(id);
            if (logProgramacionDetalle == null)
            {
                return NotFound();
            }

            _mapper.Map(updateLogProgramacionDetalleDto, logProgramacionDetalle);
            _context.Entry(logProgramacionDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogProgramacionDetalleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateLogProgramacionDetalleDto);
        }

        // post batch
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddLogProgramacionDetalle([FromBody] BatchAddLogProgramacionDetalleDto batchLogProgramacionDetalle)
        {
            if (batchLogProgramacionDetalle == null || batchLogProgramacionDetalle.logProgramacionDetalle == null || !batchLogProgramacionDetalle.logProgramacionDetalle.Any())
            {
                return BadRequest("No se proporcionaron detalles de programación para agregar.");
            }

            var logProgramacionDetalles = batchLogProgramacionDetalle.logProgramacionDetalle.Select(c => _mapper.Map<logProgramacionDetalle>(c)).ToList();
            await _context.logProgramacionDetalle.AddRangeAsync(logProgramacionDetalles);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar los detalles de programación: {ex.Message}");
            }

            return Ok(new { message = "Detalles de programación agregados exitosamente", logProgramacionDetalle = logProgramacionDetalles });
        }

        // put batch
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateLogProgramacionDetalle([FromBody] BatchUpdateLogProgramacionDetalleDto batchLogProgramacionDetalle)
        {
            if (batchLogProgramacionDetalle == null || batchLogProgramacionDetalle.logProgramacionDetalle == null || !batchLogProgramacionDetalle.logProgramacionDetalle.Any())
            {
                return BadRequest("No se proporcionaron detalles de programación para actualizar.");
            }
            foreach (var detalleDto in batchLogProgramacionDetalle.logProgramacionDetalle)
            {
                var logProgramacionDetalle = await _context.logProgramacionDetalle.FindAsync(detalleDto.idLogProgramacionDetalle);
                if (logProgramacionDetalle == null)
                {
                    return NotFound($"No se encontró el detalle de programación con ID {detalleDto.idLogProgramacionDetalle}.");
                }
                _mapper.Map(detalleDto, logProgramacionDetalle);
                _context.Entry(logProgramacionDetalle).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, $"Error al actualizar los detalles de programación: {ex.Message}");
            }
            return Ok(new { message = "Detalles de programación actualizados exitosamente", logs = batchLogProgramacionDetalle.logProgramacionDetalle });
        }

        private bool LogProgramacionDetalleExists(int id)
        {
            return _context.logProgramacionDetalle.Any(e => e.idLogProgramacionDetalle == id);
        }

    }
}
