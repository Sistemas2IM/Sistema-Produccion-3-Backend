using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf.BatchEtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta.BatchTarjetaEtiqueta;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.EtiquetaOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class etiquetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public etiquetaOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/etiquetaOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EtiquetaOfDto>>> GetetiquetaOf()
        {
            var etiquetaOf = await _context.etiquetaOf.ToListAsync();
            var etiquetaOfDto = _mapper.Map<List<EtiquetaOfDto>>(etiquetaOf);

            return (etiquetaOfDto);
        }

        // GET: api/etiquetaOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EtiquetaOfDto>> GetetiquetaOf(int id)
        {
            var etiquetaOf = await _context.etiquetaOf.FindAsync(id);
            var etiqeutaOfDto = _mapper.Map<EtiquetaOfDto>(etiquetaOf);

            if (etiqeutaOfDto == null)
            {
                return NotFound($"No se encontro la etiqueta con id {id}");
            }

            return Ok(etiqeutaOfDto);
        }

        // PUT: api/etiquetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, UpdateEtiquetaOf updateEtiquetaOf)
        {
            var etiquetaOf = await _context.etiquetaOf.FindAsync(id);
            if (etiquetaOf == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(updateEtiquetaOf, etiquetaOf);
            _context.Entry(etiquetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!etiquetaOfExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateEtiquetaOf);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateTarjetaEtiquetaOf([FromBody] BatchUpdateEtiquetaOf batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchEtiquetaOf == null || !batchUpdateDto.updateBatchEtiquetaOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchEtiquetaOf.Select(t => t.idEtiquetaOf).ToList();

            // Obtener todos los roles relacionados
            var etiquetas = await _context.etiquetaOf.Where(t => ids.Contains(t.idEtiquetaOf)).ToListAsync();

            if (!etiquetas.Any())
            {
                return NotFound("No se encontraron etiquetas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchEtiquetaOf)
            {
                var etiqueta = etiquetas.FirstOrDefault(t => t.idEtiquetaOf == dto.idEtiquetaOf);
                if (etiqueta != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idEtiquetaOf != 0)
                    {
                        etiqueta.oF = dto.oF;
                        etiqueta.idEtiqueta = dto.idEtiqueta;
                    }

                    _context.Entry(etiqueta).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las etiquetas.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        // POST: api/etiquetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<etiquetaOf>> PostetiquetaOf(AddEtiquetaOf addEtiquetaOf)
        {
            var etiquetaOf = _mapper.Map<etiquetaOf>(addEtiquetaOf);
            _context.etiquetaOf.Add(etiquetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetetiquetaOf", new { id = etiquetaOf.idEtiquetaOf }, etiquetaOf);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddTarjetaEtiqueta([FromBody] BatchAddEtiquetaOf batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchEtiquetaOf == null || !batchAddDto.addBatchEtiquetaOf.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var tarjetaEtiqueta = batchAddDto.addBatchEtiquetaOf.Select(dto => _mapper.Map<etiquetaOf>(dto)).ToList();

            await _context.etiquetaOf.AddRangeAsync(tarjetaEtiqueta);

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
                etiquetas = tarjetaEtiqueta
            });
        }

        // DELETE: api/etiquetaOf/batch
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("delete/batch")]
        public async Task<IActionResult> DeleteEtiquetasOf([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No se proporcionaron IDs válidos para eliminar.");
            }

            var etiquetasToDelete = await _context.etiquetaOf
                .Where(e => ids.Contains(e.idEtiquetaOf))
                .ToListAsync();

            if (etiquetasToDelete == null || !etiquetasToDelete.Any())
            {
                return NotFound("No se encontraron etiquetas con los IDs proporcionados.");
            }

            _context.etiquetaOf.RemoveRange(etiquetasToDelete);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar la excepción en caso de concurrencia
                return StatusCode(500, "Error al eliminar las etiquetas.");
            }

            return Ok($"Se eliminaron etiquetas correctamente.");
        }

        private bool etiquetaOfExists(int id)
        {
            return _context.etiquetaOf.Any(e => e.idEtiquetaOf == id);
        }
    }
}
