using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor;
using Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor.Batch;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.SecuenciaColor
{
    [Route("api/[controller]")]
    [ApiController]
    public class secuenciaColorController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public secuenciaColorController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<secuenciaColorController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SecuenciaColorDto>>> GetSecuenciaColor()
        {
            var secuenciaColor = await _context.secuenciaColor.ToListAsync();

            var secuenciaColorDto = _mapper.Map<List<SecuenciaColorDto>>(secuenciaColor);

            return Ok(secuenciaColorDto);
        }

        // GET api/<secuenciaColorController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SecuenciaColorDto>> GetSecuenciaColor(int id)
        {
            var secuenciaColor = await _context.secuenciaColor.FindAsync(id);

            if (secuenciaColor == null)
            {
                return NotFound();
            }
            var secuenciaColorDto = _mapper.Map<SecuenciaColorDto>(secuenciaColor);

            return Ok(secuenciaColorDto);
        }

        // POST api/<secuenciaColorController>
        [HttpPost("post")]
        public async Task<ActionResult<secuenciaColor>> PostSecuenciaColor(AddSecuenciaColorDto addSecuenciaColorDto)
        {
            var secuenciaColor = _mapper.Map<secuenciaColor>(addSecuenciaColorDto);

            _context.secuenciaColor.Add(secuenciaColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSecuenciaColor", new { id = secuenciaColor.idSecuenciaColor }, secuenciaColor);
        }

        // POST BATCH
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchPostSecuenciaColor([FromBody] BatchAddSecuenciaColor batchAddDto)
        {
            if (batchAddDto.secuenciaColor == null || !batchAddDto.secuenciaColor.Any())
            {
                return BadRequest("No se proporcionaron datos de secuenciaColor para agregar.");
            }

            var secuenciaColor = batchAddDto.secuenciaColor.Select(dto => _mapper.Map<secuenciaColor>(dto)).ToList();
            await _context.secuenciaColor.AddRangeAsync(secuenciaColor);

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
                Message = "SecuenciaColor agregados exitosamente",
                SecuenciasAgregadas = secuenciaColor
            });
        }

        // PUT api/<secuenciaColorController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutSecuenciaSolor(int id, UpdateSecuenciaColorDto updateSecuenciaColorDto)
        {
            var secuenciaColor = await _context.secuenciaColor.FindAsync(id);
            if (secuenciaColor == null)
            {
                return NotFound();
            }

            _mapper.Map(updateSecuenciaColorDto, secuenciaColor);
            _context.Entry(secuenciaColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecuenciaColorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateSecuenciaColorDto);
        }

        // PUT BATCH
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchPutSecuenciaColor([FromBody] BatchUpdateSecuenciaColor batchUpdateDto)
        {
            if (batchUpdateDto.secuenciaColor == null || !batchUpdateDto.secuenciaColor.Any())
            {
                return BadRequest("No se proporcionaron datos de secuenciaColor para actualizar.");
            }
            foreach (var updateDto in batchUpdateDto.secuenciaColor)
            {
                var secuenciaColor = await _context.secuenciaColor.FindAsync(updateDto.idSecuenciaColor);
                if (secuenciaColor == null)
                {
                    return NotFound($"No se encontró la secuenciaColor con ID {updateDto.idSecuenciaColor}.");
                }
                _mapper.Map(updateDto, secuenciaColor);
                _context.Entry(secuenciaColor).State = EntityState.Modified;
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
                Message = "SecuenciaColor actualizados exitosamente",
                SecuenciasActualizadas = batchUpdateDto.secuenciaColor
            });
        }

        private bool SecuenciaColorExists(int id)
        {
            return _context.secuenciaColor.Any(e => e.idSecuenciaColor == id);
        }
    }
}
