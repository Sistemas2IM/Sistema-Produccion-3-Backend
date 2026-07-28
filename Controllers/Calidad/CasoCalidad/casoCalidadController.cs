using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoCalidad;
using AutoMapper;

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
        public async Task<ActionResult<IEnumerable<CasoCalidadDto>>> GetCasoCalidad()
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
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
                .Include(c => c.casoAccionSolicitada)
                .Where(c => c.archivado == false && c.cancelado == false)
                .ToListAsync();

            var casoCalidadDto = _mapper.Map<List<CasoCalidadDto>>(casoCalidad);

            return Ok(casoCalidadDto);
        }

        // GET api/<casoCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CasoCalidadDto>> GetCasoCalidad(int id)
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
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
                .Include(c => c.casoAccionSolicitada)
                .Where(c => c.archivado == false && c.cancelado == false)
                .FirstOrDefaultAsync(u => u.idCasoCalidad == id);
            if (casoCalidad == null)
            {
                return NotFound();
            }
            var casoCalidadDto = _mapper.Map<CasoCalidadDto>(casoCalidad);
            return Ok(casoCalidadDto);
        }

        // GET por oF
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadDto>>> GetCasoCalidadByOF(int of)
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
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
                .Include(c => c.casoAccionSolicitada)
                .Where(c => c.oFNavigation.oF == of)
                .ToListAsync();

            var casoCalidadDto = _mapper.Map<List<CasoCalidadDto>>(casoCalidad);

            return Ok(casoCalidadDto);
        }

        // GET por oF abiertos
        [HttpGet("get/of/abiertos/{of}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadDto>>> GetCasoCalidadByOFAbiertos(int of)
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
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
                .Include(c => c.casoAccionSolicitada)
                .Where(c => c.oFNavigation.oF == of && c.idEstadoNavigation.nombreEstado == "Abierto")
                .ToListAsync();
            var casoCalidadDto = _mapper.Map<List<CasoCalidadDto>>(casoCalidad);
            return Ok(casoCalidadDto);
        }

        // GET por idProceso
        [HttpGet("get/proceso/{idProceso}")]
        public async Task<ActionResult<IEnumerable<CasoCalidadDto>>> GetCasoCalidadByProceso(int idProceso)
        {
            var casoCalidad = await _context.casoCalidad
                .OrderByDescending(c => c.idCasoCalidad)
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
                .Include(c => c.casoAccionSolicitada)
                .Where(c => c.idProceso == idProceso)
                .ToListAsync();

            var casoCalidadDto = _mapper.Map<List<CasoCalidadDto>>(casoCalidad);
            return Ok(casoCalidadDto);
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
                .Include(c => c.idProcesoNavigation)
                    .ThenInclude(t => t.idTableroNavigation)
                    .ThenInclude(m => m.idMaquinaNavigation)
                .Include(c => c.registradoPorNavigation)
                .Include(c => c.responsableNavigation)
                .Include(c => c.actualizadoPorNavigation)
                .Include(c => c.bitacoraCaso)
                .Include(c => c.casoAccionSolicitada)
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

    }
}
