using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.AuditoriaProceso.DetalleAuditoriaProceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleAuditoriaProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleAuditoriaProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<detalleAuditoriaProcesoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleAuditoriaProcesoDto>>> GetDetalleAuditoriaProceso()
        {
            var detalleAuditoriaProceso = await _context.detalleAuditoriaProceso.ToListAsync();

            var detalleAuditoriaProcesoDto = _mapper.Map<List<DetalleAuditoriaProcesoDto>>(detalleAuditoriaProceso);

            return Ok(detalleAuditoriaProcesoDto);
        }

        // GET api/<detalleAuditoriaProcesoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleAuditoriaProcesoDto>> GetDetalleAuditoriaProceso(int id)
        {
            var detalleAuditoriaProceso = await _context.detalleAuditoriaProceso.FirstOrDefaultAsync(u => u.idDetalle == id);

            if (detalleAuditoriaProceso == null)
            {
                return NotFound();
            }

            var detalleAuditoriaProcesoDto = _mapper.Map<DetalleAuditoriaProcesoDto>(detalleAuditoriaProceso);
            return Ok(detalleAuditoriaProcesoDto);
        }

        // POST api/<detalleAuditoriaProcesoController>
        [HttpPost("post")]
        public async Task<ActionResult<detalleAuditoriaProceso>> PostDetalleAuditoriaProceso(AddDetalleAuditoriaProcesoDto addDetalleAuditoriaProcesoDto)
        {
            var detalleAuditoriaProceso = _mapper.Map<detalleAuditoriaProceso>(addDetalleAuditoriaProcesoDto);

            _context.detalleAuditoriaProceso.Add(detalleAuditoriaProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleAuditoriaProceso", new { id = detalleAuditoriaProceso.idDetalle }, detalleAuditoriaProceso);
        }

        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddDetalleAuditoria([FromBody] BatchAddDetalleAuditoriaProceso batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.detalleAuditoriaProcesos == null || !batchAddDto.detalleAuditoriaProcesos.Any())
            {
                return BadRequest();
            }

            var detalleAuditoriaProcesos = batchAddDto.detalleAuditoriaProcesos.Select(dto => _mapper.Map<detalleAuditoriaProceso>(dto)).ToList();

            await _context.detalleAuditoriaProceso.AddRangeAsync(detalleAuditoriaProcesos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            return Ok(new { 
            Message = "Detalles de auditoría agregados exitosamente",
            DetallesAgregados = detalleAuditoriaProcesos
            });
        }

        // PUT api/<detalleAuditoriaProcesoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleAuditoriaProceso(int id, UpdateDetalleAuditoriaProcesoDto updateDetalleAuditoriaProcesoDto)
        {
            var detalleAuditoriaProceso = await _context.detalleAuditoriaProceso.FindAsync(id);

            if (detalleAuditoriaProceso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleAuditoriaProcesoDto, detalleAuditoriaProceso);
            _context.Entry(detalleAuditoriaProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleAuditoriaProcesoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleAuditoriaProcesoDto);
        }

        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateDetalleAuditoria([FromBody] BatchUpdateDetalleAuditoriaProceso batchUpdateDto)
        {
            if (batchUpdateDto == null || !batchUpdateDto.detalleAuditoriaProcesos.Any())
            {
                return BadRequest();
            }
            foreach (var updateDto in batchUpdateDto.detalleAuditoriaProcesos)
            {
                var detalleAuditoriaProceso = await _context.detalleAuditoriaProceso.FindAsync(updateDto.idDetalle);
                if (detalleAuditoriaProceso == null)
                {
                    return NotFound($"No se encontró el detalle de auditoría con ID {updateDto.idDetalle}");
                }
                _mapper.Map(updateDto, detalleAuditoriaProceso);
                _context.Entry(detalleAuditoriaProceso).State = EntityState.Modified;
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
                Message = "Detalles de auditoría actualizados exitosamente",
                DetallesActualizados = batchUpdateDto.detalleAuditoriaProcesos
            });
        }

        private bool detalleAuditoriaProcesoExists(int id)
        {
            return _context.detalleAuditoriaProceso.Any(e => e.idDetalle == id);
        }
    }
}
