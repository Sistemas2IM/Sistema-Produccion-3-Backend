using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque.DetalleValidacionArranque;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque.DetalleValidacionArranque.Batch;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ValidacionArranque.DetalleValidacionArranque
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleValidacionArranqueController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleValidacionArranqueController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<detalleValidacionArranqueController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleValidacionArranqueDto>>> GetDetalleValidacionArranque()
        {
            var detalleValidacionArranque = await _context.detalleValidacionArranque.ToListAsync();

            var detalleValidacionArranqueDto = _mapper.Map<List<DetalleValidacionArranqueDto>>(detalleValidacionArranque);

            return Ok(detalleValidacionArranqueDto);
        }

        // GET api/<detalleValidacionArranqueController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleValidacionArranqueDto>> GetDetalleValidacionArranque(int id)
        {
            var detalleValidacionArranque = await _context.detalleValidacionArranque.FindAsync(id);

            if (detalleValidacionArranque == null)
            {
                return NotFound();
            }

            var detalleValidacionArranqueDto = _mapper.Map<DetalleValidacionArranqueDto>(detalleValidacionArranque);

            return Ok(detalleValidacionArranqueDto);
        }

        // POST api/<detalleValidacionArranqueController>
        [HttpPost("post")]
        public async Task<ActionResult<DetalleValidacionArranqueDto>> PostDetalleValidacionArranque(AddDetalleValidacionArranqueDto addDetalleValidacionArranqueDto)
        {
            var detalleValidacionArranque = _mapper.Map<detalleValidacionArranque>(addDetalleValidacionArranqueDto);

            _context.detalleValidacionArranque.Add(detalleValidacionArranque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleValidacionArranque", new { id = detalleValidacionArranque.idDetalleValidacion }, detalleValidacionArranque);
        }

        // PUT api/<detalleValidacionArranqueController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleValidacionArranque(int id, UpdateDetalleValidacionArranqueDto updateDetalleValidacionArranqueDto)
        {
            var detalleValidacionArranque = await _context.detalleValidacionArranque.FindAsync(id);
            if (detalleValidacionArranque == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleValidacionArranqueDto, detalleValidacionArranque);
            _context.Entry(detalleValidacionArranque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleValidacionArranqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleValidacionArranqueDto);
        }

        // POST BATCH
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleArranque([FromBody] BatchAddDetalleVArranqueDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchDetalleVArranqueDto == null || !batchAddDto.addBatchDetalleVArranqueDto.Any())
            {
                return BadRequest("Estructura/datos incorrectos");
            }

            var detalleArranque = batchAddDto.addBatchDetalleVArranqueDto.Select(dto => _mapper.Map<detalleValidacionArranque>(dto)).ToList();

            await _context.detalleValidacionArranque.AddRangeAsync(detalleArranque);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Batch insert exitoso",
                detalleArranque = detalleArranque
            });
        }

        // PUT BATCH
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleArranque([FromBody] BatchUpdateDetalleVArranqueDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchDetalleVArranqueDto == null || !batchUpdateDto.updateBatchDetalleVArranqueDto.Any())
            {
                return BadRequest("Estructura/datos incorrectos");
            }

            var ids = batchUpdateDto.updateBatchDetalleVArranqueDto.Select(dto => dto.idDetalleValidacion).ToList();

            var existingEntities = await _context.detalleValidacionArranque.Where(e => ids.Contains(e.idDetalleValidacion)).ToListAsync();

            // Si la cantidad de encontrados no es igual a la cantidad de solicitados, entonces sí falta alguno.
            if (existingEntities.Count != ids.Count)
            {
                return BadRequest("Algunas entidades no existen");
            }

            foreach (var updateDto in batchUpdateDto.updateBatchDetalleVArranqueDto)
            {
                var entity = existingEntities.FirstOrDefault(e => e.idDetalleValidacion == updateDto.idDetalleValidacion);
                if (entity != null)
                {
                    _mapper.Map(updateDto, entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Batch update exitoso",
                DetallesActualizados = batchUpdateDto.updateBatchDetalleVArranqueDto
            });
        }

        private bool DetalleValidacionArranqueExists(int id)
        {
            return _context.detalleValidacionArranque.Any(e => e.idDetalleValidacion == id);
        }
    }
}
