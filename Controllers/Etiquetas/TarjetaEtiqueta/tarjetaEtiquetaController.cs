using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta.BatchTarjetaEtiqueta;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.TarjetaEtiqueta
{
    [Route("api/[controller]")]
    [ApiController]
    public class tarjetaEtiquetaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;   

        public tarjetaEtiquetaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tarjetaEtiqueta
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaEtiquetaDto>>> GettarjetaEtiqueta()
        {
            var tarjetaEtiqueta = await _context.tarjetaEtiqueta.ToListAsync();
            var tarjetaEtiquetaDto = _mapper.Map<List<TarjetaEtiquetaDto>>(tarjetaEtiqueta);

            return (tarjetaEtiquetaDto);
        }

        // GET: api/tarjetaEtiqueta/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TarjetaEtiquetaDto>> GettarjetaEtiqueta(int id)
        {
           var tarjetaEtiqueta = await _context.tarjetaEtiqueta
                .FirstOrDefaultAsync(u => u.idTarjetaEtiqueta == id);

            var tarjetaEtiquetaDto = _mapper.Map<TarjetaEtiquetaDto>(tarjetaEtiqueta);

            if (tarjetaEtiquetaDto == null)
            {
                return NotFound($"No se encontro la etiqueta de tarjeta con id {id}");
            }
            return Ok(tarjetaEtiquetaDto);
        }

        // PUT: api/tarjetaEtiqueta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttarjetaEtiqueta(int id, UpdateTarjetaEtiquetaDto updateTarjetaEtiqueta)
        {
            var tarjetaEtiqueta = await _context.tarjetaEtiqueta.FindAsync(id);

            if (tarjetaEtiqueta == null)
            {
                return NotFound($"No se encontro la etiqueta de tarjeta con id {id}");
            }

            _mapper.Map(updateTarjetaEtiqueta, tarjetaEtiqueta);
            _context.Entry(tarjetaEtiqueta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tarjetaEtiquetaExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateTarjetaEtiqueta);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateTarjetaEtiqueta([FromBody] BatchUpdateTarjetaEtiqueta batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchTarjetaEtiquetas == null || !batchUpdateDto.updateBatchTarjetaEtiquetas.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchTarjetaEtiquetas.Select(t => t.idTarjetaEtiqueta).ToList();

            // Obtener todos los roles relacionados
            var etiquetas = await _context.tarjetaEtiqueta.Where(t => ids.Contains(t.idTarjetaEtiqueta)).ToListAsync();

            if (!etiquetas.Any())
            {
                return NotFound("No se encontraron etiquetas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchTarjetaEtiquetas)
            {
                var etiqueta = etiquetas.FirstOrDefault(t => t.idTarjetaEtiqueta == dto.idTarjetaEtiqueta);
                if (etiqueta != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idTarjetaEtiqueta != 0)
                    {
                        etiqueta.idEtiqueta = dto.idEtiqueta.Value;
                        etiqueta.idProceso = dto.idProceso.Value;
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

        // POST: api/tarjetaEtiqueta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<tarjetaEtiqueta>> PosttarjetaEtiqueta(AddTarjetaEtiquetaDto addTarjetaEtiqueta)
        {
            var tarjetaEtiqueta = _mapper.Map<tarjetaEtiqueta>(addTarjetaEtiqueta);
            _context.tarjetaEtiqueta.Add(tarjetaEtiqueta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettarjetaEtiqueta", new { id = tarjetaEtiqueta.idTarjetaEtiqueta }, tarjetaEtiqueta);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddTarjetaEtiqueta([FromBody] BatchAddTarjetaEtiqueta batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.batchAddTarjetaEtiqueta == null || !batchAddDto.batchAddTarjetaEtiqueta.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var tarjetaEtiqueta = batchAddDto.batchAddTarjetaEtiqueta.Select(dto => _mapper.Map<tarjetaEtiqueta>(dto)).ToList();

            await _context.tarjetaEtiqueta.AddRangeAsync(tarjetaEtiqueta);

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

        private bool tarjetaEtiquetaExists(int id)
        {
            return _context.tarjetaEtiqueta.Any(e => e.idTarjetaEtiqueta == id);
        }
    }
}
