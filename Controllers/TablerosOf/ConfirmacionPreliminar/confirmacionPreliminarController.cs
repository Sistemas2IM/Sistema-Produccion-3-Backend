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
        public async Task<ActionResult<IEnumerable<ConfirmacionPreliminarListaDTO>>> GetConfirmacionPreliminar()
        {
            var confirmacionPreliminar = await _context.confirmacionPreliminar
                .AsNoTracking()
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(p => p.idTableroNavigation)
                        .ThenInclude(t => t.idMaquinaNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(p => p.corridaCombinadamaestroNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(p => p.corridaCombinadasubordinadoNavigation)
                .Include(c => c.idUnidadNavigation)
                .Include(c => c.idTurnoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.operadorNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.entregadoPorNavigation)
                .Include(c => c.transferenciaProceso)
                .Include(c => c.conciliacion)
                .AsSplitQuery()
                .Where(c => c.archivado == false)
                .OrderByDescending(c => c.idPreliminar)
                .ToListAsync();

            // 1. Extraer los procesos válidos
            var procesos = confirmacionPreliminar
                .Where(c => c.idProcesoNavigation != null)
                .Select(c => c.idProcesoNavigation)
                .ToList();

            // 2. Extraer todos los IDs de subordinados necesarios de un solo golpe
            var idsSubordinados = procesos
                .SelectMany(p => p.corridaCombinadamaestroNavigation?.Select(c => c.subordinado) ?? Enumerable.Empty<int?>())
                .Union(procesos.Select(p => p.corridaCombinadasubordinadoNavigation?.subordinado))
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .Distinct()
                .ToList();

            // 3. Consultar la BD solo 1 vez para traer todos los subordinados
            var subordinadosDict = new Dictionary<int, procesoOf>();
            if (idsSubordinados.Any())
            {
                subordinadosDict = await _context.procesoOf
                    .AsNoTracking()
                    .Include(p => p.oFNavigation)
                    .Where(p => idsSubordinados.Contains(p.idProceso))
                    .ToDictionaryAsync(p => p.idProceso);
            }

            // 4. Asignación en memoria (Instantáneo)
            foreach (var proceso in procesos)
            {
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado.HasValue && subordinadosDict.TryGetValue(corrida.subordinado.Value, out var sub))
                        {
                            corrida.subordinadoNavigation = sub;
                        }
                    }
                }

                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    if (subordinadosDict.TryGetValue(proceso.corridaCombinadasubordinadoNavigation.subordinado.Value, out var sub))
                    {
                        proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = sub;
                    }
                }
            }

            // 🚀 5. CONSULTAR TRANSFERENCIAS PENDIENTES EN BLOQUE
            // Como idProceso no es nulo, lo seleccionamos directamente
            var idsProcesos = procesos.Select(p => p.idProceso).Distinct().ToList();
            var conteoTransferenciasPendientes = new Dictionary<int, int>();

            if (idsProcesos.Any())
            {
                conteoTransferenciasPendientes = await _context.transferenciaProceso
                    .AsNoTracking()
                    .Where(t => t.estado == "Pendiente" && t.idDestino.HasValue && idsProcesos.Contains(t.idDestino.Value))
                    .GroupBy(t => t.idDestino.Value)
                    .Select(g => new {
                        idDestino = g.Key,
                        cantidad = g.Count()
                    })
                    .ToDictionaryAsync(x => x.idDestino, x => x.cantidad);
            }

            // 6. Mapeo al DTO final
            var confirmacionPreliminarDto = confirmacionPreliminar.Select(c =>
            {
                var dto = _mapper.Map<ConfirmacionPreliminarListaDTO>(c);
                dto.totalTransferencias = c.transferenciaProceso?.Count ?? 0;
                dto.cantidadEnviada = c.transferenciaProceso?.Sum(t => (decimal?)t.cantidadEnviada) ?? 0;
                dto.cantidadConfirmada = c.transferenciaProceso?.Sum(t => (decimal?)t.cantidadConfirmada) ?? 0;
                dto.saldo = (dto.cantidadRecibida ?? 0) - (c.transferenciaProceso?.Sum(t => (decimal?)t.cantidadEnviada ?? 0) ?? 0);

                // 🚀 1. Buscamos el ID en el diccionario (si no existe, devuelve 0 por defecto)
                int pendientes = conteoTransferenciasPendientes.GetValueOrDefault(c.idProceso, 0);

                // 🚀 2. Asignamos los dos campos nuevos simultáneamente
                dto.cantidadTransferenciasPendientes = pendientes;
                dto.transferenciaDisponible = pendientes > 0;

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

            confirmacionPreliminarDto.saldo = (confirmacionPreliminarDto.cantidadRecibida ?? 0)
                - (confirmacionPreliminar.transferenciaProceso?.Sum(t => (decimal?)t.cantidadEnviada) ?? 0);

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
        public async Task<ActionResult<ConfirmacionPreliminarCreateResponseDTO>> PostConfirmacionPreliminar(AddConfirmacionPreliminarDto addConfirmacionPreliminarDto)
        {
            var confirmacionPreliminar = _mapper.Map<confirmacionPreliminar>(addConfirmacionPreliminarDto);

            _context.confirmacionPreliminar.Add(confirmacionPreliminar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfirmacionPreliminar", 
                new { id = confirmacionPreliminar.idPreliminar }, 
                new ConfirmacionPreliminarCreateResponseDTO
                {
                    idPreliminar = confirmacionPreliminar.idPreliminar,
                    Message = "Se ha creado correctamente el registro de confirmacionPreliminar."
                });
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
