using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.AuditoriaProceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class auditoriaProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public auditoriaProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<auditoriaProcesoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AuditoriaProcesoDto>>> GetAuditoriaProceso()
        {
            var auditoriaProceso = await _context.auditoriaProceso
                .Include(a => a.detalleAuditoriaProceso)
                .ToListAsync();

            var auditoriaProcesoDto = _mapper.Map<List<AuditoriaProcesoDto>>(auditoriaProceso);

            return Ok(auditoriaProcesoDto);
        }

        // GET api/<auditoriaProcesoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<AuditoriaProcesoDto>> GetAuditoriaProceso(int id)
        {
            var auditoriaProceso = await _context.auditoriaProceso
                .Include(a => a.detalleAuditoriaProceso)
                .FirstOrDefaultAsync(u => u.idAuditoria == id);

            if (auditoriaProceso == null)
            {
                return NotFound();
            }
            var auditoriaProcesoDto = _mapper.Map<AuditoriaProcesoDto>(auditoriaProceso);

            return Ok(auditoriaProcesoDto);
        }

        // GET: api/<auditoriaProcesoController>
        [HttpGet("get/turno/{turno}")]
        public async Task<ActionResult<IEnumerable<AuditoriaProcesoDto>>> GetAuditoriaProcesoByTurno(int turno)
        {
            var auditoriaProceso = await _context.auditoriaProceso
                .Include(a => a.detalleAuditoriaProceso)
                .Where(a => a.turnoAuditoria == turno)
                .ToListAsync();

            var auditoriaProcesoDto = _mapper.Map<List<AuditoriaProcesoDto>>(auditoriaProceso);

            return Ok(auditoriaProcesoDto);
        }

        // POST api/<auditoriaProcesoController>
        [HttpPost("post")]
        public async Task<ActionResult<auditoriaProceso>> PostAuditoriaProceso(AddAuditoriaProcesoDto addAuditoriaProcesoDto)
        {
            var auditoriaProceso = _mapper.Map<auditoriaProceso>(addAuditoriaProcesoDto);

            _context.auditoriaProceso.Add(auditoriaProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuditoriaProceso", new { id = auditoriaProceso.idAuditoria }, auditoriaProceso);
        }

        // PUT api/<auditoriaProcesoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutAuditoriaProceso(int id, UpdateAuditoriaProcesoDto updateAuditoriaProcesoDto)
        {
            var auditoriaProceso = await _context.auditoriaProceso.FindAsync(id);

            if (auditoriaProceso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateAuditoriaProcesoDto, auditoriaProceso);
            _context.Entry(auditoriaProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!auditoriaProcesoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateAuditoriaProcesoDto);
        }

        private bool auditoriaProcesoExists(int id)
        {
            return _context.auditoriaProceso.Any(e => e.idAuditoria == id);

        }
    }
}
