using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaCliente.DetalleFichaClientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleFichaClientesController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleFichaClientesController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<detalleFichaClientesController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleFichaClientesDto>>> GetDetalleFichaClientes()
        {
            var detalleFichaClientes = await _context.detalleFichaClientes
                .Include(v => v.idVariableNavigation)
                .Include(u => u.idUnidadNavigation)
                .ToListAsync();

            var detalleFichaClientesDto = _mapper.Map<List<DetalleFichaClientesDto>>(detalleFichaClientes);

            return Ok(detalleFichaClientesDto);
        }

        // GET api/<detalleFichaClientesController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleFichaClientesDto>> GetDetalleFichaClientes(int id)
        {
            var detalleFichaClientes = await _context.detalleFichaClientes
                .Include(v => v.idVariableNavigation)
                .Include(u => u.idUnidadNavigation)
                .FirstOrDefaultAsync(u => u.idDetalle == id);

            if (detalleFichaClientes == null)
            {
                return NotFound();
            }

            var detalleFichaClientesDto = _mapper.Map<DetalleFichaClientesDto>(detalleFichaClientes);

            return Ok(detalleFichaClientesDto);
        }

        // POST api/<detalleFichaClientesController>
        [HttpPost("post")]
        public async Task<ActionResult<detalleFichaClientes>> PostDetalleFichaClientes(AddDetalleFichaClientesDto detalleFichaClientesDto)
        {
            var detalleFichaClientes = _mapper.Map<detalleFichaClientes>(detalleFichaClientesDto);

            _context.detalleFichaClientes.Add(detalleFichaClientes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleFichaClientes", new { id = detalleFichaClientes.idDetalle }, detalleFichaClientes);
        }

        // PUT api/<detalleFichaClientesController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleFichaCliente(int id, UpdateDetalleFichaClientesDto updateDetalleFichaClientesDto)
        {
            var detalleFichaClientes = await _context.detalleFichaClientes.FindAsync(id);

            if (detalleFichaClientes == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleFichaClientesDto, detalleFichaClientes);
            _context.Entry(detalleFichaClientes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleFichaClientesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleFichaClientesDto);
        }

        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleFichaCliente([FromBody] List<AddBatchDetalleFichaClientesDto> batchDetalleFichaDto)
        {
            if (batchDetalleFichaDto == null || !batchDetalleFichaDto.Any())
            {
                return BadRequest("La lista de detalles está vacía o no se proporcionó.");
            }

            var detalleFicha = _mapper.Map<List<detalleFichaClientes>>(batchDetalleFichaDto);

            await _context.detalleFichaClientes.AddRangeAsync(detalleFicha);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al guardar los detalles: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Detalles agregados exitosamente",
                TotalAgregados = detalleFicha.Count,
                DetallesAgregados = detalleFicha
            });
        }

        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleFichaCliente([FromBody] List<UpdateBatchDetalleFichaClientesDto> updateDetalleFichaDto)
        {
            if (updateDetalleFichaDto == null || !updateDetalleFichaDto.Any())
            {
                return BadRequest("La lista de detalles está vacía o no se proporcionó.");
            }

            var resultados = new List<object>();

            foreach (var dto in updateDetalleFichaDto)
            {
                // Validar ID
                if (dto.idDetalle <= 0)
                {
                    resultados.Add(new
                    {
                        id = dto.idDetalle,
                        estado = "Error",
                        mensaje = "ID inválido."
                    });
                    continue;
                }

                // Buscar registro existente
                var existente = await _context.detalleFichaClientes
                    .FirstOrDefaultAsync(x => x.idDetalle == dto.idDetalle);

                if (existente == null)
                {
                    resultados.Add(new
                    {
                        id = dto.idDetalle,
                        estado = "Error",
                        mensaje = "Detalle no encontrado."
                    });
                    continue;
                }

                // Mapear los cambios (AutoMapper sobrescribe los campos modificados)
                _mapper.Map(dto, existente);

                // Actualizar en contexto
                _context.detalleFichaClientes.Update(existente);

                resultados.Add(new
                {
                    id = dto.idDetalle,
                    estado = "OK",
                    mensaje = "Actualizado correctamente."
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al actualizar los detalles: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Proceso de actualización por lote finalizado.",
                Resultados = resultados
            });
        }

        private bool detalleFichaClientesExists(int id)
        {
            return _context.detalleFichaClientes.Any(e => e.idDetalle == id);
        }
    }
}
