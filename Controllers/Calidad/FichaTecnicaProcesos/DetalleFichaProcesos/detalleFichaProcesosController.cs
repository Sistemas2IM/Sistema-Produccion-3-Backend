using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleFichaProcesosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public DetalleFichaProcesosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<DetalleFichaProcesosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleFichaProcesosDto>>> GetDetalleProcesos()
        {
            var detalleProcesos = await _context.detalleFichaProcesos.ToListAsync();

            var detalleProcesosDto = _mapper.Map<List<DetalleFichaProcesosDto>>(detalleProcesos);

            return Ok(detalleProcesosDto);
        }

        // GET api/<DetalleFichaProcesosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleFichaProcesosDto>> GetDetalleProcesos(int id)
        {
            var detalleProcesos = await _context.detalleFichaProcesos.FindAsync(id);

            if (detalleProcesos == null)
            {
                return NotFound();
            }
            var detalleProcesosDto = _mapper.Map<DetalleFichaProcesosDto>(detalleProcesos);

            return Ok(detalleProcesosDto);
        }
        // POST api/<DetalleFichaProcesosController>
        [HttpPost("post")]
        public async Task<ActionResult<detalleFichaProcesos>> PostDetalleProcesos(AddDetalleFichaProcesosDto addDetalleFichaProcesosDto)
        {
            var detalleProcesos = _mapper.Map<detalleFichaProcesos>(addDetalleFichaProcesosDto);

            _context.detalleFichaProcesos.Add(detalleProcesos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleProcesos", new { id = detalleProcesos.idDetalle }, detalleProcesos);
        }

        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleFichaProcesos([FromBody] BatchAddDetalleFichaProcesos batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.detalleFichaProcesos == null || !batchAddDto.detalleFichaProcesos.Any())
            {
                return BadRequest("No se proporcionaron datos para agregar.");
            }

            var detalleProcesosList = batchAddDto.detalleFichaProcesos.Select(dto => _mapper.Map<detalleFichaProcesos>(dto)).ToList();
            await _context.detalleFichaProcesos.AddRangeAsync(detalleProcesosList);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Los detalles de ficha procesos se agregaron correctamente.",
                DetallesAgregados = detalleProcesosList
            });
        }

        // PUT api/<DetalleFichaProcesosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleProceso(int id, UpdateDetalleFichaProcesosDto updateDetalleFichaProcesosDto)
        {
            var detalleProcesos = await _context.detalleFichaProcesos.FindAsync(id);
            if (detalleProcesos == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleFichaProcesosDto, detalleProcesos);
            _context.Entry(detalleProcesos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleProcesoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleFichaProcesosDto);

        }

        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleFichaProcesos([FromBody] BatchUpdateDetalleFichaProcesos batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.detalleFichaProcesos == null || !batchUpdateDto.detalleFichaProcesos.Any())
            {
                return BadRequest("No se proporcionaron datos para actualizar.");
            }
            foreach (var updateDto in batchUpdateDto.detalleFichaProcesos)
            {
                var detalleProceso = await _context.detalleFichaProcesos.FindAsync(updateDto.idDetalle);
                if (detalleProceso == null)
                {
                    return NotFound($"No se encontró el detalle de proceso con ID {updateDto.idDetalle}.");
                }
                _mapper.Map(updateDto, detalleProceso);
                _context.Entry(detalleProceso).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al actualizar: {ex.Message}");
            }
            return Ok(new
            {
                Message = "Los detalles de ficha procesos se actualizaron correctamente.",
                DetallesActualizados = batchUpdateDto.detalleFichaProcesos
            });
        }

        private bool DetalleProcesoExists(int id)
        {
            return _context.detalleFichaProcesos.Any(e => e.idDetalle == id);
        }
    }
}
