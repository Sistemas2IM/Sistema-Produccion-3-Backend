using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CertificadoCalidad.DetalleCertificadoCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleCertificadoCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleCertificadoCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<detalleCertificadoCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleCertificadoCalidadDto>>> GetDetalleCertificadoCalidad()
        {
            var detalleCertificadoCalidad = await _context.detalleCertificadoCalidad
                .Include(v => v.idVariableNavigation)
                .Include(u => u.idUnidadNavigation)
                .ToListAsync();

            var detalleCertificadoCalidadDto = _mapper.Map<List<DetalleCertificadoCalidadDto>>(detalleCertificadoCalidad);

            return Ok(detalleCertificadoCalidadDto);
        }

        // GET api/<detalleCertificadoCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleCertificadoCalidadDto>> Get(int id)
        {
            var detalleCertificadoCalidad = await _context.detalleCertificadoCalidad
                .Include(v => v.idVariableNavigation)
                .Include(u => u.idUnidadNavigation)
                .FirstOrDefaultAsync(u => u.idDetalle == id);
            if (detalleCertificadoCalidad == null)
            {
                return NotFound();
            }
            var detalleCertificadoCalidadDto = _mapper.Map<DetalleCertificadoCalidadDto>(detalleCertificadoCalidad);
            return Ok(detalleCertificadoCalidadDto);
        }

        // POST api/<detalleCertificadoCalidadController>
        [HttpPost("post")]
        public async Task<ActionResult<DetalleCertificadoCalidadDto>> Post([FromBody] AddDetalleCertificadoCalidadDto addDetalleCertificadoCalidadDto)
        {
            var detalleCertificadoCalidad = _mapper.Map<detalleCertificadoCalidad>(addDetalleCertificadoCalidadDto);

            _context.detalleCertificadoCalidad.Add(detalleCertificadoCalidad);
            await _context.SaveChangesAsync();

            var detalleCertificadoCalidadDto = _mapper.Map<DetalleCertificadoCalidadDto>(detalleCertificadoCalidad);

            return CreatedAtAction("Get", new { id = detalleCertificadoCalidad.idDetalle }, detalleCertificadoCalidadDto);
        }

        // PUT api/<detalleCertificadoCalidadController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleCertificadoCalidad(int id, UpdateDetalleCertificadoCalidadDto updateDetalleCertificadoCalidadDto)
        {
            var detalleCertificadoCalidad = await _context.detalleCertificadoCalidad.FindAsync(id);
            if (detalleCertificadoCalidad == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleCertificadoCalidadDto, detalleCertificadoCalidad);
            _context.Entry(detalleCertificadoCalidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleCertificadoCalidadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleCertificadoCalidadDto);
        }

        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleCertificado([FromBody] List<AddBatchDetalleCertificadoC> detallesDto)
        {
            if (detallesDto == null || !detallesDto.Any())
            {
                return BadRequest("La lista de detalles está vacía o no se proporcionó.");
            }

            var detalles = _mapper.Map<List<detalleCertificadoCalidad>>(detallesDto);

            await _context.detalleCertificadoCalidad.AddRangeAsync(detalles);

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
                TotalAgregados = detalles.Count,
                DetallesAgregados = detalles
            });
        }

        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleCertificado([FromBody] List<UpdateBatchDetalleCertificadoC> detallesDto)
        {
            if (detallesDto == null || !detallesDto.Any())
            {
                return BadRequest("La lista de detalles está vacía o no se proporcionó.");
            }

            var resultados = new List<object>();

            foreach (var dto in detallesDto)
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
                var existente = await _context.detalleCertificadoCalidad
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
                _context.detalleCertificadoCalidad.Update(existente);

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

        private bool detalleCertificadoCalidadExists(int id)
        {
            return _context.detalleCertificadoCalidad.Any(e => e.idDetalle == id);
        }
    }
}
