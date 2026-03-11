using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.RegistroLamparas
{
    [Route("api/[controller]")]
    [ApiController]
    public class registroLamparasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public registroLamparasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<registroLamparasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<RegistroLamparasDto>>> GetRegistroLamparas()
        {
            var registroLamparas = await _context.registroLamparas.ToListAsync();

            var registroLamparasDto = _mapper.Map<List<RegistroLamparasDto>>(registroLamparas);

            return Ok(registroLamparasDto);
        }

        // GET api/<registroLamparasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<RegistroLamparasDto>> GetRegistroLamparas(int id)
        {
            var registroLamparas = await _context.registroLamparas.FindAsync(id);

            if (registroLamparas == null)
            {
                return NotFound();
            }

            var registroLamparasDto = _mapper.Map<RegistroLamparasDto>(registroLamparas);

            return Ok(registroLamparasDto);
        }

        // POST api/<registroLamparasController>
        [HttpPost("post")]
        public async Task<ActionResult> PostRegistroLamparas(AddRegistroLamparasDto addRegistroLamparasDto)
        {
            var registroLamparas = _mapper.Map<registroLamparas>(addRegistroLamparasDto);

            _context.registroLamparas.Add(registroLamparas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroLamparas", new { id = registroLamparas.idRegistroLampara }, addRegistroLamparasDto);
        }

        // POST BATCH
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddRegistroLamparas([FromBody] BatchAddRegistroLamparas batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.registrosLamparas == null || !batchAddDto.registrosLamparas.Any())
            {
                return BadRequest("No se proporcionaron registros de lámparas para agregar.");
            }

            var registrosLamparas = batchAddDto.registrosLamparas.Select(dto => _mapper.Map<registroLamparas>(dto)).ToList();

            await _context.registroLamparas.AddRangeAsync(registrosLamparas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            return Ok(new { 
                Message = "Registros de lámparas agregados exitosamente", 
                RegistrosAgregados = registrosLamparas 
            });
        }

        // PUT api/<registroLamparasController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutRegistroLamparas(int id, UpdateRegistroLamparasDto updateRegistroLamparasDto)
        {
            var registroLamparas = await _context.registroLamparas.FindAsync(id);

            if (registroLamparas == null)
            {
                return NotFound();
            }

            _mapper.Map(updateRegistroLamparasDto, registroLamparas);
            _context.Entry(registroLamparas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroLamparasExists(id))
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

        //PUT BATCH
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateRegistroLamparas([FromBody] BatchUpdateRegistroLamparas batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.registrosLamparas == null || !batchUpdateDto.registrosLamparas.Any())
            {
                return BadRequest("No se proporcionaron registros de lámparas para actualizar.");
            }
            foreach (var updateDto in batchUpdateDto.registrosLamparas)
            {
                var registroLamparas = await _context.registroLamparas.FindAsync(updateDto.idRegistroLampara);
                if (registroLamparas == null)
                {
                    return NotFound($"No se encontró el registro de lámpara con ID {updateDto.idRegistroLampara}.");
                }
                _mapper.Map(updateDto, registroLamparas);
                _context.Entry(registroLamparas).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al actualizar: {ex.Message}");
            }
            return Ok(new { 
                Message = "Registros de lámparas actualizados exitosamente",
                RegistrosActualizados = batchUpdateDto.registrosLamparas
            });
        }

        private bool RegistroLamparasExists(int id)
        {
            return _context.registroLamparas.Any(e => e.idRegistroLampara == id);
        }
    }
}
