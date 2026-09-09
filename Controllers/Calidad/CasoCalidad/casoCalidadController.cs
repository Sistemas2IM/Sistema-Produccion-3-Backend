using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoCalidad;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Acabado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.AcabadoFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Barnizado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Impresión;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.ImpresionFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.MangaFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Pegadora;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Preprensa;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Serigrafia;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Troquelado;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CasoCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class casoCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public casoCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<casoCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CasoCalidadListaDTO>>> GetCasoCalidad()
        {
            var casos = await CasosParaLista()
                .Where(c => c.archivado == false && c.cancelado == false)
                .OrderByDescending(c => c.idCasoCalidad)
                .Include(c => c.unidadMedidaNavigation)
                .ToListAsync();

            return Ok(MapearLista(casos));
        }

        // GET api/<casoCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CasoCalidadDto>> GetCasoCalidad(int id)
        {
            try
            {
                var casoCalidad = await _context.casoCalidad
                    .AsNoTracking()
                    .Include(c => c.idTipoCasoNavigation)
                    .Include(c => c.idEstadoNavigation)
                    .Include(c => c.idSeveridadNavigation)
                    .Include(c => c.idCategoriaDefectoNavigation)
                    .Include(c => c.idSubtipoDefectoNavigation)
                    .Include(c => c.oFNavigation)
                    .Include(c => c.idProcesoNavigation)
                        .ThenInclude(t => t.idTableroNavigation)
                        .ThenInclude(m => m.idMaquinaNavigation)
                    .Include(c => c.idProcesoNavigation)
                        .ThenInclude(m => m.idMaterialNavigation)
                    // INCLUDES DE MÁQUINAS
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoImpresora)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoAcabado)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoBarniz)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoPegadora)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoMangaFlexo)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoImpresoraFlexo)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoAcabadoFlexo)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoTroqueladora)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoPreprensa)
                    .Include(c => c.idProcesoNavigation).ThenInclude(p => p.procesoSerigrafia)
                    // RESTO DE INCLUDES
                    .Include(c => c.registradoPorNavigation)
                    .Include(c => c.responsableNavigation)
                    .Include(c => c.actualizadoPorNavigation)
                    .Include(c => c.bitacoraCaso)
                        .ThenInclude(b => b.idTipoEventoNavigation)
                    .Include(c => c.bitacoraCaso)
                        .ThenInclude(b => b.estadoAnteriorNavigation)
                    .Include(c => c.bitacoraCaso)
                        .ThenInclude(b => b.estadoNuevoNavigation)
                    .Include(c => c.bitacoraCaso)
                        .ThenInclude(b => b.idDictamenNavigation)
                    .Include(c => c.bitacoraCaso)
                        .ThenInclude(b => b.idAnexoNavigation)
                    .Include(c => c.casoAccionSolicitada)
                        .ThenInclude(a => a.idAccionNavigation)
                    .Include(c => c.casoAccionSolicitada)
                        .ThenInclude(a => a.idEstadoNavigation)
                    .Include(c => c.idCausaRaizNavigation)
                    .Include(c => c.idResolucionNavigation)
                    .Include(c => c.areaResponsableNavigation)
                    .Include(c => c.unidadMedidaNavigation)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(u => u.idCasoCalidad == id);

                if (casoCalidad == null) return NotFound();

                var casoCalidadDto = _mapper.Map<CasoCalidadDto>(casoCalidad);

                // 🚀 EVALUACIÓN DINÁMICA: Mapeamos la máquina correcta al objeto genérico
                // 🚀 EVALUACIÓN DINÁMICA: Extrayendo el primer registro de la lista
                var p = casoCalidad.idProcesoNavigation;
                if (p != null)
                {
                    if (p.procesoImpresora != null && p.procesoImpresora.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoImpresoraDto>(p.procesoImpresora.FirstOrDefault());

                    else if (p.procesoAcabado != null && p.procesoAcabado.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoAcabadoDto>(p.procesoAcabado.FirstOrDefault());

                    else if (p.procesoBarniz != null && p.procesoBarniz.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoBarnizDto>(p.procesoBarniz.FirstOrDefault());

                    else if (p.procesoPegadora != null && p.procesoPegadora.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoPegadoraDto>(p.procesoPegadora.FirstOrDefault());

                    else if (p.procesoMangaFlexo != null && p.procesoMangaFlexo.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoMangaFlexoDto>(p.procesoMangaFlexo.FirstOrDefault());

                    else if (p.procesoImpresoraFlexo != null && p.procesoImpresoraFlexo.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoImpresoraFlexoDto>(p.procesoImpresoraFlexo.FirstOrDefault());

                    else if (p.procesoAcabadoFlexo != null && p.procesoAcabadoFlexo.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoAcabadoFlexoDto>(p.procesoAcabadoFlexo.FirstOrDefault());

                    else if (p.procesoTroqueladora != null && p.procesoTroqueladora.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(p.procesoTroqueladora.FirstOrDefault());

                    else if (p.procesoPreprensa != null && p.procesoPreprensa.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoPreprensaDto>(p.procesoPreprensa.FirstOrDefault());

                    else if (p.procesoSerigrafia != null && p.procesoSerigrafia.Any())
                        casoCalidadDto.detalleProceso = _mapper.Map<ProcesoSerigrafiaDto>(p.procesoSerigrafia.FirstOrDefault());
                }

                return Ok(casoCalidadDto);
            }
            catch (Exception ex)
            {
                // Si hay una referencia circular u otro fallo, lo verás directamente en Swagger
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    DetalleInterno = ex.InnerException?.Message
                });
            }
        }

        // GET por oF
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadListaDTO>>> GetCasoCalidadByOF(int of)
        {
            var casos = await CasosParaLista()
                .Where(c => c.oF == of && c.archivado == false && c.cancelado == false)
                .OrderByDescending(c => c.idCasoCalidad)
                .ToListAsync();

            return Ok(MapearLista(casos));
        }

        // GET por oF abiertos
        [HttpGet("get/of/abiertos/{of}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadListaDTO>>> GetCasoCalidadByOFAbiertos(int of)
        {
            var casos = await CasosParaLista()
                .Where(c => c.oF == of
                         && c.archivado == false
                         && c.cancelado == false
                         && c.idEstadoNavigation.nombreEstado == "Abierto")
                .OrderByDescending(c => c.idCasoCalidad)
                .ToListAsync();

            return Ok(MapearLista(casos));
        }

        // GET por idProceso
        [HttpGet("get/proceso/{idProceso}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadListaDTO>>> GetCasoCalidadByProceso(int idProceso)
        {
            var casos = await CasosParaLista()
                .Where(c => c.idProceso == idProceso && c.archivado == false && c.cancelado == false)
                .OrderByDescending(c => c.idCasoCalidad)
                .ToListAsync();

            return Ok(MapearLista(casos));
        }

        // GET agrupado por estado, con los objetos de cada estado, y la cantidad de casos por estado
        [HttpGet("get/estado")]
        public async Task<ActionResult<IEnumerable<object>>> GetCasoCalidadByEstado()
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
                .Include(c => c.idTipoCasoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.idSeveridadNavigation)
                .Include(c => c.idCategoriaDefectoNavigation)
                .Include(c => c.idSubtipoDefectoNavigation)
                .Include(c => c.oFNavigation)
                .Include(c => c.idCausaRaizNavigation)
                .Include(c => c.idResolucionNavigation)
                .Include(c => c.areaResponsableNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(t => t.idTableroNavigation)
                    .ThenInclude(m => m.idMaquinaNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.responsableNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idTipoEventoNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idDictamenNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idAnexoNavigation)
                .Include(c => c.casoAccionSolicitada)
                .Include(c => c.unidadMedidaNavigation)
                .Where(c => c.archivado == false && c.cancelado == false)
                .ToListAsync();
            var casoCalidadDto = _mapper.Map<List<CasoCalidadDto>>(casoCalidad);
            var result = casoCalidadDto
                .GroupBy(c => new { c.idEstado, c.nombreEstado })
                .Select(g => new
                {
                    idEstado = g.Key.idEstado,
                    nombreEstado = g.Key.nombreEstado,
                    cantidadCasos = g.Count(),
                    casos = g.ToList()
                })
                .ToList();
            return Ok(result);
        }

        // POST api/<casoCalidadController>
        [HttpPost("post")]
        public async Task<ActionResult<CasoCalidadCreateResponseDTO>> PostCasoCalidad(AddCasoCalidadDto casoCalidadDto)
        {
            var entidad = _mapper.Map<casoCalidad>(casoCalidadDto);

            _context.casoCalidad.Add(entidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCasoCalidad),
                new { id = entidad.idCasoCalidad },
                new CasoCalidadCreateResponseDTO
                {
                    IdCasoCalidad = entidad.idCasoCalidad,
                    Message = "Caso de calidad creado correctamente."
                });
        }

        // PUT api/<casoCalidadController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCasoCalidad(int id, UpdateCasoCalidadDto updateCasoCalidadDto)
        {
            var casoCalidad = await _context.casoCalidad.FindAsync(id);

            if (casoCalidad == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCasoCalidadDto, casoCalidad);
            _context.Entry(casoCalidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasoCalidadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCasoCalidadDto);
        }

        private bool CasoCalidadExists(int id)
        {
            return (_context.casoCalidad?.Any(e => e.idCasoCalidad == id)).GetValueOrDefault();
        }

        // Consulta base con las relaciones que necesita CasoCalidadListaDto.
        // Un solo lugar: si mañana falta un Include, se arregla acá y aplica a los 4 listados.
        private IQueryable<casoCalidad> CasosParaLista() =>
            _context.casoCalidad
                .AsNoTracking()
                .Include(c => c.idTipoCasoNavigation)
                .Include(c => c.idEstadoNavigation)
                .Include(c => c.idSeveridadNavigation)
                .Include(c => c.idCategoriaDefectoNavigation)
                .Include(c => c.idSubtipoDefectoNavigation)
                .Include(c => c.oFNavigation)
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(t => t.idTableroNavigation)
                    .ThenInclude(m => m.idMaquinaNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.responsableNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idTipoEventoNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idDictamenNavigation)
                .Include(c => c.bitacoraCaso)
                    .ThenInclude(b => b.idAnexoNavigation)
                .Include(c => c.casoAccionSolicitada)
                .AsSplitQuery();

        // Mapeo + los dos contadores
        private List<CasoCalidadListaDTO> MapearLista(List<casoCalidad> casos) =>
            casos.Select(c =>
            {
                var dto = _mapper.Map<CasoCalidadListaDTO>(c);
                dto.totalEventos = c.bitacoraCaso?.Count ?? 0;
                dto.totalAcciones = c.casoAccionSolicitada?.Count ?? 0;
                return dto;
            }).ToList();

    }
}
