using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria;
using Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.TurnoAuditoria
{
    [Route("api/[controller]")]
    [ApiController]
    public class turnoAuditoriaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public turnoAuditoriaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<turnoAuditoriaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TurnoAuditoriaDto>>> GetTurnoAuditoria()
        {
            var turnoAuditoria = await _context.turnoAuditoria
                .Include(t => t.aprobadoPorNavigation)
                .Include(t => t.auditorNavigation)
                .Include(t => t.turnoNavigation)
                .Include(t => t.estadoNavigation)
                .ToListAsync();

            var turnoAuditoriaDto = _mapper.Map<List<TurnoAuditoriaDto>>(turnoAuditoria);

            return Ok(turnoAuditoriaDto);
        }

        // GET api/<turnoAuditoriaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TurnoAuditoriaDto>> GetTurnoAuditoria(int id)
        {
            var turnoAuditoria = await _context.turnoAuditoria
                .Include(t => t.aprobadoPorNavigation)
                .Include(t => t.auditorNavigation)
                .Include(t => t.turnoNavigation)
                .Include(t => t.estadoNavigation)
                .FirstOrDefaultAsync(u => u.idTurnoAuditor == id);

            if (turnoAuditoria == null)
            {
                return NotFound();
            }
            var turnoAuditoriaDto = _mapper.Map<TurnoAuditoriaDto>(turnoAuditoria);

            return Ok(turnoAuditoriaDto);
        }

        // POST api/<turnoAuditoriaController>
        [HttpPost("post")]
        public async Task<ActionResult<turnoAuditoria>> PostTurnoAuditoria(AddTurnoAuditoriaDto addTurnoAuditoriaDto)
        {
            var turnoAuditoria = _mapper.Map<turnoAuditoria>(addTurnoAuditoriaDto);

            _context.turnoAuditoria.Add(turnoAuditoria);
            await _context.SaveChangesAsync();

            return Ok(turnoAuditoria);
        }

        // PUT api/<turnoAuditoriaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTurnoAuditoria(int id, UpdateTurnoAuditoriaDto updateTurnoAuditoriaDto)
        {
            var turnoAuditoria = await _context.turnoAuditoria.FindAsync(id);
            if (turnoAuditoria == null)
            {
                return NotFound();
            }
            _mapper.Map(updateTurnoAuditoriaDto, turnoAuditoria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!turnoAuditoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // PUT BATCH ESTADO
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchUpdateTurnoAuditor([FromBody] BatchUpdateTurnoAuditoriaDto batchUpdateDto)
        {
            if (batchUpdateDto.TurnoAuditoriaUpdates == null || !batchUpdateDto.TurnoAuditoriaUpdates.Any())
            {
                return BadRequest("No se proporcionaron actualizaciones.");
            }
            var idsToUpdate = batchUpdateDto.TurnoAuditoriaUpdates.Select(u => u.idTurnoAuditor).ToList();
            var turnoAuditoriaList = await _context.turnoAuditoria.Where(t => idsToUpdate.Contains(t.idTurnoAuditor)).ToListAsync();
            foreach (var update in batchUpdateDto.TurnoAuditoriaUpdates)
            {
                var turnoAuditoria = turnoAuditoriaList.FirstOrDefault(t => t.idTurnoAuditor == update.idTurnoAuditor);
                if (turnoAuditoria != null)
                {
                    _mapper.Map(update, turnoAuditoria);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar los registros.");
            }
            return Ok("Actualización realizada correctamente");
        }

        private bool turnoAuditoriaExists(int id)
        {
            return _context.turnoAuditoria.Any(e => e.idTurnoAuditor == id);
        }
    }
}
