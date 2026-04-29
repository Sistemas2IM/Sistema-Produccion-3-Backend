using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Sistema_Produccion_3_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud.Batch;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.EtiquetaSolicitud
{
    [Route("api/[controller]")]
    [ApiController]
    public class etiquetaSolicitudController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        public etiquetaSolicitudController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<etiquetaSolicitudController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EtiquetaSolicitudDto>>> GetEtiquetaSolicitud()
        {
            var etiquetaSolicitud = await _context.etiquetaSolicitud.ToListAsync();
            var etiquetaSolicitudDto = _mapper.Map<List<EtiquetaSolicitudDto>>(etiquetaSolicitud);

            return (etiquetaSolicitudDto);
        }

        // GET api/<etiquetaSolicitudController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EtiquetaSolicitudDto>> GetEtiquetaSolicitud(int id)
        {
            var etiquetaSolicitud = await _context.etiquetaSolicitud
                 .FirstOrDefaultAsync(u => u.idEtiqSolicitud == id);

            var etiquetaSolicitudDto = _mapper.Map<EtiquetaSolicitudDto>(etiquetaSolicitud);

            if (etiquetaSolicitudDto == null)
            {
                return NotFound($"No se encontro la etiqueta de solicitud con id {id}");
            }

            return Ok(etiquetaSolicitudDto);
        }

        // POST api/<etiquetaSolicitudController>
        [HttpPost("post")]
        public async Task<ActionResult<etiquetaSolicitud>> PostEtiquetaSolicitud(AddEtiquetaSolicitudDto addEtiquetaSolicitud)
        {
            var etiquetaSolicitud = _mapper.Map<etiquetaSolicitud>(addEtiquetaSolicitud);
            _context.etiquetaSolicitud.Add(etiquetaSolicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEtiquetaSolicitud", new { id = etiquetaSolicitud.idEtiqSolicitud }, etiquetaSolicitud);
        }

        // PUT api/<etiquetaSolicitudController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutEtiquetaSolicitud(int id, UpdateEtiquetaSolicitudDto updateEtiquetaSolicitud)
        {
            var etiquetaSolicitud = await _context.etiquetaSolicitud.FindAsync(id);

            if (etiquetaSolicitud == null)
            {
                return NotFound($"No se encontro la etiqueta de solicitud con id {id}");
            }

            _mapper.Map(updateEtiquetaSolicitud, etiquetaSolicitud);
            _context.Entry(etiquetaSolicitud).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!etiquetaSolicitudExists(id))
                {
                    return NotFound($"No se encontro la etiqueta de solicitud con id {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateEtiquetaSolicitud);
        }

        // POST BATCH
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchAddEtiquetaSolicitud([FromBody] BatchAddEtiquetaSolicitudDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchEtiquetaSolicitud == null || !batchAddDto.addBatchEtiquetaSolicitud.Any())
            {
                return BadRequest("La lista de etiquetas de solicitud está vacía o es nula.");
            }

            var etiquetaSolicitud = batchAddDto.addBatchEtiquetaSolicitud.Select(dto => _mapper.Map<etiquetaSolicitud>(dto)).ToList();

            await _context.etiquetaSolicitud.AddRangeAsync(etiquetaSolicitud);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al guardar las etiquetas de solicitud: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Las etiquetas de solicitud se agregaron correctamente.",
                etiquetas = etiquetaSolicitud,
            });
        }

        // PUT BATCH
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateEtiquetaSolicitud([FromBody] BatchUpdateEtiquetaSolicitudDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchEtiquetaSolicitud == null || !batchUpdateDto.updateBatchEtiquetaSolicitud.Any())
            {
                return BadRequest("La lista de etiquetas de solicitud está vacía o es nula.");
            }
            var updateResults = new List<object>();
            foreach (var updateDto in batchUpdateDto.updateBatchEtiquetaSolicitud)
            {
                var etiquetaSolicitud = await _context.etiquetaSolicitud.FindAsync(updateDto.idEtiqSolicitud);
                if (etiquetaSolicitud == null)
                {
                    updateResults.Add(new { id = updateDto.idEtiqSolicitud, Status = "Not Found" });
                    continue;
                }
                _mapper.Map(updateDto, etiquetaSolicitud);
                _context.Entry(etiquetaSolicitud).State = EntityState.Modified;
                updateResults.Add(new { id = updateDto.idEtiqSolicitud, Status = "Updated" });
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar las etiquetas de solicitud: {ex.Message}");
            }
            return Ok(new
            {
                Message = "Las etiquetas de solicitud se actualizaron correctamente.",
                Results = updateResults,
            });
        }

        //DELETE BATCH
        [HttpDelete("delete/Batch")]
        public async Task<IActionResult> DeleteEtiquetaSolicitud([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("La lista de IDs está vacía o es nula.");
            }

            var etiquetasSolicitud = await _context.etiquetaSolicitud
                .Where(e => ids.Contains(e.idEtiqSolicitud))
                .ToListAsync();

            if (etiquetasSolicitud == null || !etiquetasSolicitud.Any())
            {
                return NotFound("No se encontraron etiquetas de solicitud con los IDs proporcionados.");
            }

            _context.etiquetaSolicitud.RemoveRange(etiquetasSolicitud);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar las etiquetas de solicitud: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Las etiquetas de solicitud se eliminaron correctamente.",
                DeletedIds = etiquetasSolicitud.Select(e => e.idEtiqSolicitud).ToList(),
            });
        }

        private bool etiquetaSolicitudExists(int id)
        {
            return _context.etiquetaSolicitud.Any(e => e.idEtiqSolicitud == id);
        }
    }
}
