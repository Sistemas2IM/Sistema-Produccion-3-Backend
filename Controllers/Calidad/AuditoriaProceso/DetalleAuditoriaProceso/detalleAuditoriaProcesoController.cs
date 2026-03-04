using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso;
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

            return Ok(updateDetalleAuditoriaProcesoDto)
        }

        private bool detalleAuditoriaProcesoExists(int id)
        {
            return _context.detalleAuditoriaProceso.Any(e => e.idDetalle == id);
        }
}
