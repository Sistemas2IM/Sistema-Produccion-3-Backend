using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales.Conciliacion
{
    [Route("api/[controller]")]
    [ApiController]
    public class conciliacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public conciliacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<conciliacionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ConciliacionDto>>> GetConciliaciones()
        {
            var conciliaciones = await _context.conciliacion
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(p => p.idProcesoNavigation)
                        .ThenInclude(t => t.idTableroNavigation)
                            .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(op => op.operadorNavigation)
                .Include(c => c.idMotivoNavigation)
                .Include(c => c.idDecisionNavigation)
                .Include(c => c.responsableNavigation)
                .Where(c => c.archivado == false)
                .ToListAsync();

            var conciliacionesDto = _mapper.Map<List<ConciliacionDto>>(conciliaciones);

            return Ok(conciliacionesDto);
        }

        // GET api/<conciliacionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ConciliacionDto>> GetConciliacion(int id)
        {
            var conciliacion = await _context.conciliacion
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(p => p.idProcesoNavigation)
                        .ThenInclude(t => t.idTableroNavigation)
                            .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(op => op.operadorNavigation)
                .Include(c => c.idMotivoNavigation)
                .Include(c => c.idDecisionNavigation)
                .Include(c => c.responsableNavigation)
                .Where(c => c.idConciliacion == id)
                .FirstOrDefaultAsync();
            if (conciliacion == null)
            {
                return NotFound();
            }
            var conciliacionDto = _mapper.Map<ConciliacionDto>(conciliacion);
            return Ok(conciliacionDto);
        }

        // GET por idProceso
        [HttpGet("get/proceso/{idProceso}")]
        public async Task<ActionResult<IEnumerable<ConciliacionDto>>> GetConciliacionesPorProceso(int idProceso)
        {
            var conciliaciones = await _context.conciliacion
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(p => p.idProcesoNavigation)
                        .ThenInclude(t => t.idTableroNavigation)
                            .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(op => op.operadorNavigation)
                .Include(c => c.idMotivoNavigation)
                .Include(c => c.idDecisionNavigation)
                .Include(c => c.responsableNavigation)
                .Where(c => c.idPreliminarNavigation.idProceso == idProceso)
                .ToListAsync();

            var conciliacionesDto = _mapper.Map<List<ConciliacionDto>>(conciliaciones);

            return Ok(conciliacionesDto);
        }

        // GET idPreliminar
        [HttpGet("get/preliminar/{idPreliminar}")]
        public async Task<ActionResult<IEnumerable<ConciliacionDto>>> GetConciliacionesPorPreliminar(int idPreliminar)
        {
            var conciliaciones = await _context.conciliacion
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(p => p.idProcesoNavigation)
                        .ThenInclude(t => t.idTableroNavigation)
                            .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idPreliminarNavigation)
                    .ThenInclude(op => op.operadorNavigation)
                .Include(c => c.idMotivoNavigation)
                .Include(c => c.idDecisionNavigation)
                .Include(c => c.responsableNavigation)
                .Where(c => c.idPreliminar == idPreliminar)
                .ToListAsync();
            var conciliacionesDto = _mapper.Map<List<ConciliacionDto>>(conciliaciones);
            return Ok(conciliacionesDto);
        }

        // POST api/<conciliacionController>
        [HttpPost("post")]
        public async Task<ActionResult<ConciliacionCreateResponseDTO>> PostConciliacion(AddConciliacionDto addConciliacionDto)
        {
            var conciliacion = _mapper.Map<conciliacion>(addConciliacionDto);

            _context.conciliacion.Add(conciliacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConciliacion),
                new { id= conciliacion.idConciliacion},
                new ConciliacionCreateResponseDTO
                {
                    IdConciliacion = conciliacion.idConciliacion,
                    Message = "Conciliación creada exitosamente."
                });
        }

        // PUT api/<conciliacionController>/5
        [HttpPut("put/{id}")]
       public async Task<IActionResult> PutConciliacion(int id, UpdateConciliacionDto updateConciliacionDto)
        {
            var conciliacion = await _context.conciliacion.FindAsync(id);

            if (conciliacion == null)
            {
                return NotFound();
            }

            _mapper.Map(updateConciliacionDto, conciliacion);
            _context.Entry(conciliacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConciliacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateConciliacionDto);
        }

        private bool ConciliacionExists(int id)
        {
            return (_context.conciliacion?.Any(e => e.idConciliacion == id)).GetValueOrDefault();
        }
    }
}
