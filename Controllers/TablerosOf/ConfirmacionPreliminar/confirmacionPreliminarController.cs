using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf.ConfirmacionPreliminar
{
    [Route("api/[controller]")]
    [ApiController]
    public class confirmacionPreliminarController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public confirmacionPreliminarController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<confirmacionPreliminarController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ConfirmacionPreliminarDto>>> GetConfirmacionPreliminar()
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso) // Incluir la colección de transferencias
                .Where(c => c.archivado == false)
                .ToListAsync();

            // Mapear y calcular campos calculados
            var confirmacionPreliminarDto = confirmacionPreliminar.Select(c =>
            {
                var dto = _mapper.Map<ConfirmacionPreliminarDto>(c);
                dto.totalTransferencias = c.transferenciaProceso?.Count ?? 0;
                dto.saldo = (dto.cantidadRecibida ?? 0) - dto.totalTransferencias;
                return dto;
            }).ToList();

            return Ok(confirmacionPreliminarDto);
        }


        // GET api/<confirmacionPreliminarController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ConfirmacionPreliminarDto>> GetConfirmacionPreliminar(int id)
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .AsNoTracking()
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(p => p.idTableroNavigation)
                        .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso)
                .Include(c => c.conciliacion)
                    .ThenInclude(x => x.idMotivoNavigation)
                .Include(c => c.conciliacion)
                    .ThenInclude(x => x.idDecisionNavigation)
                .Include(c => c.conciliacion)
                    .ThenInclude(x => x.responsableNavigation)
                .FirstOrDefaultAsync(u => u.idPreliminar == id);

            if (confirmacionPreliminar == null)
            {
                return NotFound();
            }

            var confirmacionPreliminarDto = _mapper.Map<ConfirmacionPreliminarDto>(confirmacionPreliminar);
            confirmacionPreliminarDto.totalTransferencias = confirmacionPreliminar.transferenciaProceso?.Count ?? 0;
            confirmacionPreliminarDto.saldo = (confirmacionPreliminarDto.cantidadRecibida ?? 0) - confirmacionPreliminarDto.totalTransferencias;

            return Ok(confirmacionPreliminarDto);
        }

        // GET por idProceso
        [HttpGet("get/proceso/{idProceso}")]
        public async Task<ActionResult<IEnumerable<ConfirmacionPreliminarDto>>> GetConfirmacionPreliminarByProceso(int idProceso)
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso) // Incluir la colección de transferencias
                .Where(c => c.idProceso == idProceso && c.archivado == false)
                .ToListAsync();
            if (confirmacionPreliminar == null || !confirmacionPreliminar.Any())
            {
                return NotFound();
            }
            var confirmacionPreliminarDto = confirmacionPreliminar.Select(c =>
            {
                var dto = _mapper.Map<ConfirmacionPreliminarDto>(c);
                dto.totalTransferencias = c.transferenciaProceso?.Count ?? 0;
                dto.saldo = (dto.cantidadRecibida ?? 0) - dto.totalTransferencias;
                return dto;
            }).ToList();

            return Ok(confirmacionPreliminarDto);
        }

        // GET por idProceso con estado "Pendiente"
        [HttpGet("get/proceso/{idProceso}/pendientes")]
        public async Task<ActionResult<IEnumerable<ConfirmacionPreliminarDto>>> GetConfirmacionPreliminarByProcesoPendiente(int idProceso)
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso) // Incluir la colección de transferencias
                .Where(c => c.idProceso == idProceso && c.archivado == false && c.idEstadoNavigation.nombreEstado == "Pendiente")
                .ToListAsync();
            if (confirmacionPreliminar == null || !confirmacionPreliminar.Any())
            {
                return NotFound();
            }
            var confirmacionPreliminarDto = confirmacionPreliminar.Select(c =>
            {
                var dto = _mapper.Map<ConfirmacionPreliminarDto>(c);
                dto.totalTransferencias = c.transferenciaProceso?.Count ?? 0;
                dto.saldo = (dto.cantidadRecibida ?? 0) - dto.totalTransferencias;
                return dto;
            }).ToList();

            return Ok(confirmacionPreliminarDto);
        }

        // GET por oF
        [HttpGet("get/of/{oF}")]
        public async Task<ActionResult<IEnumerable<ConfirmacionPreliminarDto>>> GetConfirmacionPreliminarByOF(int oF)
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso) // Incluir la colección de transferencias
                .Where(c => c.oFNavigation.oF == oF && c.archivado == false)
                .ToListAsync();
            if (confirmacionPreliminar == null || !confirmacionPreliminar.Any())
            {
                return NotFound();
            }
            var confirmacionPreliminarDto = confirmacionPreliminar.Select(c =>
            {
                var dto = _mapper.Map<ConfirmacionPreliminarDto>(c);
                dto.totalTransferencias = c.transferenciaProceso?.Count ?? 0;
                dto.saldo = (dto.cantidadRecibida ?? 0) - dto.totalTransferencias;
                return dto;
            }).ToList();

            return Ok(confirmacionPreliminarDto);
        }

        // POST api/<confirmacionPreliminarController>
        [HttpPost("post")]
        public async Task<ActionResult<confirmacionPreliminar>> PostConfirmacionPreliminar(AddConfirmacionPreliminarDto addConfirmacionPreliminarDto)
        {
            var confirmacionPreliminar = _mapper.Map<confirmacionPreliminar>(addConfirmacionPreliminarDto);

            _context.confirmacionPreliminar.Add(confirmacionPreliminar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfirmacionPreliminar", new { id = confirmacionPreliminar.idPreliminar }, confirmacionPreliminar);
        }

        // PUT api/<confirmacionPreliminarController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutConfirmacionPreliminar(int id, UpdateConfirmacionPreliminarDto updateConfirmacionPreliminarDto)
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar.FindAsync(id);

            if (confirmacionPreliminar == null)
            {
                return NotFound();
            }

            _mapper.Map(updateConfirmacionPreliminarDto, confirmacionPreliminar);
            _context.Entry(confirmacionPreliminar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfirmacionPreliminarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateConfirmacionPreliminarDto);
        }

        private bool ConfirmacionPreliminarExists(int id)
        {
            return _context.confirmacionPreliminar.Any(e => e.idPreliminar == id);
        }
    }
}
