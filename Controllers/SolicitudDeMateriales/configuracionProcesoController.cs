using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ConfiguracionProceso;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class configuracionProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public configuracionProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<configuracionProcesoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ConfiguracionProcesoDto>>> GetConfigProceso()
        {
            var configuracionProcesos = await _context.configuracionProceso
                .ToListAsync();

            var configuracionProcesoDtos = _mapper.Map<List<ConfiguracionProcesoDto>>(configuracionProcesos);

            return Ok(configuracionProcesoDtos);
        }

        // GET api/<configuracionProcesoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<ConfiguracionProcesoDto>>> GetConfigProcesoId(int id)
        {
            var configuracionP = await _context.configuracionProceso
                .FirstOrDefaultAsync(cp => cp.idConfig == id);

            var configuracionProcesoDto = _mapper.Map<ConfiguracionProcesoDto>(configuracionP);

            if (configuracionProcesoDto == null)
            {
                return NotFound($"No se encontraon registros con el ID: {id}");
            }

            return Ok(configuracionProcesoDto);
        }

        // POST api/<configuracionProcesoController>
        [HttpPost("post")]
        public async Task<ActionResult<configuracionProceso>> PostConfigProceso(AddConfiguracionProcesoDto addConfiguracionProcesoDto)
        {
            var configuracionProceso = _mapper.Map<configuracionProceso>(addConfiguracionProcesoDto);
            _context.configuracionProceso.Add(configuracionProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfigProcesoId", new { id = configuracionProceso.idConfig }, configuracionProceso);
        }

        // PUT api/<configuracionProcesoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutConfigProceso(int id, UpdateConfiguracionProcesoDto updateConfiguracionProcesoDto)
        {
            var configuracionesProceso = await _context.configuracionProceso.FindAsync(id);
            if (configuracionesProceso == null)
            {
                return NotFound($"No se encontraron registros con el ID: {id}");
            }

            _mapper.Map(updateConfiguracionProcesoDto, configuracionesProceso);
            _context.Entry(configuracionesProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguracionProcesoExists(id))
                {
                    return NotFound($"No se encontraron registros con el ID: {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateConfiguracionProcesoDto);
        }

        private bool ConfiguracionProcesoExists(int id)
        {
            return _context.configuracionProceso.Any(e => e.idConfig == id);
        }

    }
}
