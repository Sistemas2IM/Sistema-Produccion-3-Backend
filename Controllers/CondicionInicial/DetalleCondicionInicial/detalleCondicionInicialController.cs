using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.CondicionInicial.DetalleCondicionInicial
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleCondicionInicialController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleCondicionInicialController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<detalleCondicionInicialController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleCondicionInicialDto>>> GetDetalleCondicionInicial()
        {
            var detalleCondicionInicial = await _context.detalleCondicionInicial
                .Include(vt => vt.idVariableNavigation)
                .ToListAsync();
            var detalleCondicionInicialDto = _mapper.Map<List<DetalleCondicionInicialDto>>(detalleCondicionInicial);

            return Ok(detalleCondicionInicialDto);
        }

        // GET api/<detalleCondicionInicialController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleCondicionInicialDto>> GetDetalleCondicionInicial(int id)
        {
            var detalleCondicionInicial = await _context.detalleCondicionInicial
                .Include(vt => vt.idVariableNavigation)
                .FirstOrDefaultAsync(d => d.idDetalleCondicionInicial == id);
            if (detalleCondicionInicial == null)
            {
                return NotFound();
            }
            var detalleCondicionInicialDto = _mapper.Map<DetalleCondicionInicialDto>(detalleCondicionInicial);
            return Ok(detalleCondicionInicialDto);
        }

        // POST api/<detalleCondicionInicialController>
        [HttpPost("post")]
        public async Task<ActionResult<DetalleCondicionInicialDto>> PostDetalleCondicionInicial(DetalleCondicionInicialDto detalleCondicionInicialDto)
        {
            var detalleCondicionInicial = _mapper.Map<detalleCondicionInicial>(detalleCondicionInicialDto);

            _context.detalleCondicionInicial.Add(detalleCondicionInicial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleCondicionInicial", new { id = detalleCondicionInicial.idDetalleCondicionInicial }, detalleCondicionInicialDto);
        }

        // PUT api/<detalleCondicionInicialController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleCondicionInicial(int id, UpdateDetalleCondicionInicialDto updateDetalleCondicionInicialDto)
        {
            var detalleCondicionInicial = await _context.detalleCondicionInicial.FindAsync(id);

            if (detalleCondicionInicial == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleCondicionInicialDto, detalleCondicionInicial);
            _context.Entry(detalleCondicionInicial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleCondicionInicialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleCondicionInicialDto);
        }

        // post batch
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleCondicionInicial([FromBody] BatchAddDetalleCondicionInicialDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.DetallesCondicionInicial == null || !batchAddDto.DetallesCondicionInicial.Any())
            {
                return BadRequest("No se proporcionaron datos para agregar.");
            }

            var detalleCondicionInicialDetalle = batchAddDto.DetallesCondicionInicial.Select(dto => _mapper.Map<detalleCondicionInicial>(dto)).ToList();

            await _context.detalleCondicionInicial.AddRangeAsync(detalleCondicionInicialDetalle);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al guardar los datos: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Datos agregados exitosamente.",
                detalles = detalleCondicionInicialDetalle,
            });
        }

        // put batch
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleCondicionInicial([FromBody] BatchUpdateDetalleCondicionInicialDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.DetallesCondicionInicial == null || !batchUpdateDto.DetallesCondicionInicial.Any())
            {
                return BadRequest("No se proporcionaron datos para actualizar.");
            }
            var updateResult = new List<object>();
            foreach (var updateDto in batchUpdateDto.DetallesCondicionInicial)
            {
                var detalleCondicionInicial = await _context.detalleCondicionInicial.FindAsync(updateDto.idDetalleCondicionInicial);
                if (detalleCondicionInicial == null)
                {
                    return NotFound($"No se encontró el detalle de condición inicial con id {updateDto.idDetalleCondicionInicial}");
                }
                _mapper.Map(updateDto, detalleCondicionInicial);
                _context.Entry(detalleCondicionInicial).State = EntityState.Modified;
                updateResult.Add(new { id = updateDto.idDetalleCondicionInicial, status = "Actualizado" });
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar los datos: {ex.Message}");
            }
            return Ok(new
            {
                Message = "Datos actualizados exitosamente.",
                detalles = updateResult,
            });
        }
        private bool detalleCondicionInicialExists(int id)
        {
            return _context.detalleCondicionInicial.Any(e => e.idDetalleCondicionInicial == id);
        }
    }
}
