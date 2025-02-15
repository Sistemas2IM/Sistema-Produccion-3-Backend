using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta.BathcEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta.BatchTarjetaEtiqueta;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.Etiqueta
{
    [Route("api/[controller]")]
    [ApiController]
    public class etiquetaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public etiquetaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/etiqueta
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EtiquetaDto>>> Getetiqueta()
        {
            var etiqueta = await _context.etiqueta.ToListAsync();
            var etiquetaDto = _mapper.Map<List<EtiquetaDto>>(etiqueta);

            return Ok(etiquetaDto);
        }

        // GET: api/etiqueta/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EtiquetaDto>> Getetiqueta(int id)
        {
            var etiqueta = await _context.etiqueta.FindAsync(id);
            var etiquetaDto = _mapper.Map<EtiquetaDto>(etiqueta);

            if (etiquetaDto == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            return Ok(etiquetaDto);
        }

        // PUT: api/etiqueta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putetiqueta(int id, UpdateEtiquetaDto updateEtiqueta)
        {
            var etiqueta = await _context.etiqueta.FindAsync(id);

            if (etiqueta == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(updateEtiqueta, etiqueta);
            _context.Entry(etiqueta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!etiquetaExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateEtiqueta);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateTarjetaEtiqueta([FromBody] BatchUpdateEtiqueta batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchEtiquetas == null || !batchUpdateDto.updateBatchEtiquetas.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchEtiquetas.Select(t => t.idEtiqueta).ToList();

            // Obtener todos los roles relacionados
            var etiquetas = await _context.etiqueta.Where(t => ids.Contains(t.idEtiqueta)).ToListAsync();

            if (!etiquetas.Any())
            {
                return NotFound("No se encontraron etiquetas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchEtiquetas)
            {
                var etiqueta = etiquetas.FirstOrDefault(t => t.idEtiqueta == dto.idEtiqueta);
                if (etiqueta != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idEtiqueta != 0)
                    {
                        etiqueta.color = dto.color;
                        etiqueta.texto = dto.texto;
                        etiqueta.secuencia = dto.secuencia;
                        etiqueta.flag = dto.flag;
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

        // POST: api/etiqueta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<etiqueta>> Postetiqueta(AddEtiquetaDto addEtiqueta)
        {
            var etiqueta = _mapper.Map<etiqueta>(addEtiqueta);
            _context.etiqueta.Add(etiqueta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getetiqueta", new { id = etiqueta.idEtiqueta }, etiqueta);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddTarjetaEtiqueta([FromBody] BatchAddEtiqueta batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchEtiquetas == null || !batchAddDto.addBatchEtiquetas.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var etiqueta = batchAddDto.addBatchEtiquetas.Select(dto => _mapper.Map<etiqueta>(dto)).ToList();

            await _context.etiqueta.AddRangeAsync(etiqueta);

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
                etiquetas = etiqueta
            });
        }

        private bool etiquetaExists(int id)
        {
            return _context.etiqueta.Any(e => e.idEtiqueta == id);
        }
    }
}
