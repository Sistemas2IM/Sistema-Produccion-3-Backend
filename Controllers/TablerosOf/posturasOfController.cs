using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class posturasOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public posturasOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/posturasOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PosturasOfDto>>> GetposturasOf()
        {
            var postura = await _context.posturasOf
                .Include(p => p.idTableroNavigation)
                .ToListAsync();

            var posturaDto = _mapper.Map<List<PosturasOfDto>>(postura);

            return Ok(posturaDto);
        }     

        // GET: api/posturasOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PosturasOfDto>> GetposturasOf(int id)
        {
            var postura = await _context.posturasOf
                .Include(p => p.idTableroNavigation)
                .FirstOrDefaultAsync(u => u.idPostura == id);

            var posturaDto = _mapper.Map<PosturasOfDto>(postura);

            if (posturaDto == null)
            {
                return NotFound("No se encontro la Postura con el ID: " + id);
            }

            return Ok(posturaDto);
        }

        // PUT: api/posturasOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutposturasOf(int id, UpdatePosturasOfDto updatePosturasOf)
        {
            var postura = await _context.posturasOf.FindAsync(id);

            if (postura == null)
            {
                return NotFound("No se encontro la Postura con el ID: " + id);
            }

            _mapper.Map(updatePosturasOf, postura);
            _context.Entry(postura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!posturasOfExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updatePosturasOf);
        }

        // POST: api/posturasOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<posturasOf>> PostposturasOf(AddPosturasOfDto addPosturasOf)
        {
            var postura = _mapper.Map<posturasOf>(addPosturasOf);
            _context.posturasOf.Add(postura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetposturasOf", new { id = postura.idPostura }, postura);
        }       

        private bool posturasOfExists(int id)
        {
            return _context.posturasOf.Any(e => e.idPostura == id);
        }

        // POST BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<ActionResult> BatchAddPosturasOf([FromBody] BatchAddPosturasOf batchAddPosturasOf)
        {
            if (batchAddPosturasOf == null || batchAddPosturasOf.addBatchPosturasOf == null || !batchAddPosturasOf.addBatchPosturasOf.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var posturas = batchAddPosturasOf.addBatchPosturasOf.Select(dto => _mapper.Map<posturasOf>(dto)).ToList();

            await _context.posturasOf.AddRangeAsync(posturas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar las posturas: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Posturas agregadas exitosamente.",
                posturas = posturas
            });
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdatePosturasOf([FromBody] BatchUpdatePosturaOf batchUpdatePosturaOf)
        {
            if (batchUpdatePosturaOf == null || batchUpdatePosturaOf.updateBatchPosturasOf == null || !batchUpdatePosturaOf.updateBatchPosturasOf.Any())
            {
                return BadRequest("No se encontraron posturas para los IDs proporcionados");
            }

            var ids = batchUpdatePosturaOf.updateBatchPosturasOf.Select(t => t.idPostura).ToList();

            // Obtener todas las posturas relacionadas
            var posturas = await _context.posturasOf.Where(t => ids.Contains(t.idPostura)).ToListAsync();

            if (!posturas.Any())
            {
                return NotFound("No se encontraron posturas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdatePosturaOf.updateBatchPosturasOf)
            {
                var postura = posturas.FirstOrDefault(t => t.idPostura == dto.idPostura);
                if (postura != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idPostura != 0)
                    {
                        postura.nombrePostura = dto.nombrePostura;
                        postura.secuencia = dto.secuencia;
                        postura.idTablero = dto.idTablero;
                    }
                    _context.Entry(postura).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las posturas.");
            }

            return Ok(new
            {
                Message = "Posturas actualizadas exitosamente.",
                posturas = posturas
            });
        }
    }
}
