using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf.BatchEtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.NotasOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.NotasOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class notasOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public notasOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/notasOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<NotasDto>>> GetnotasOf()
        {
            var notas = await _context.notasOf.ToListAsync();
            var notasDto = _mapper.Map<List<NotasDto>>(notas);

            return Ok(notasDto);
        }

        // GET: api/notasOf
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<NotasDto>>> GetnotasTarjeta(int of)
        {
            var notas = await _context.notasOf
                .Where(o => o.oF == of)
                .ToListAsync();
            var notasDto = _mapper.Map<List<NotasDto>>(notas);

            return Ok(notasDto);
        }

        // GET: api/notasOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<NotasDto>> GetnotasOf(int id)
        {
            var notasOf = await _context.notasOf.FindAsync(id);
            var notasDto = _mapper.Map<NotasDto>(notasOf);

            if (notasDto == null)
            {
                return NotFound($"No se encontro la nota con el id {id}");
            }

            return Ok(notasDto);
        }

        // PUT: api/notasOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutnotasOf(int id, UpdateNotasDto updateNotasOf)
        {
            var notas = await _context.notasOf.FindAsync(id);

            if (notas == null)
            {
                return NotFound($"No se encontro la nota con el id {id}");
            }

            _mapper.Map(updateNotasOf, notas);
            _context.Entry(notas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!notasOfExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateNotasOf);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateNotasOf([FromBody] BatchUpdateNotasDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchNotasOf == null || !batchUpdateDto.updateBatchNotasOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchNotasOf.Select(t => t.idComentario).ToList();

            // Obtener todos los roles relacionados
            var notas = await _context.notasOf.Where(t => ids.Contains(t.idComentario)).ToListAsync();

            if (!notas.Any())
            {
                return NotFound("No se encontraron notas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchNotasOf)
            {
                var nota = notas.FirstOrDefault(t => t.idComentario == dto.idComentario);
                if (nota != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idComentario != 0)
                    {
                        nota.texto = dto.texto;
                        nota.tipoNota = dto.tipoNota;
                    }

                    _context.Entry(nota).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las notas.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddNotaOf([FromBody] BatchAddNotasOf batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchNotasOf == null || !batchAddDto.addBatchNotasOf.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var nota = batchAddDto.addBatchNotasOf.Select(dto => _mapper.Map<notasOf>(dto)).ToList();

            await _context.notasOf.AddRangeAsync(nota);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar las etiquetas: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Etiqeutas agregados exitosamente.",
                etiquetas = nota
            });
        }

        private bool notasOfExists(int id)
        {
            return _context.notasOf.Any(e => e.idComentario == id);
        }
    }
}
