using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Horarios.IndisponibilidadMaquinas;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Horarios.IndisponibilidadMaquinas
{
    [Route("api/[controller]")]
    [ApiController]
    public class indisponibilidadMaquinasController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<indisponibilidadMaquinasController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public indisponibilidadMaquinasController(base_nuevaContext context, IMapper mapper, ILogger<indisponibilidadMaquinasController> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/<indisponibilidadMaquinasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<IndisponibilidadMaquinasDto>>> GetIndisponibilidadMaquina()
        {
            var indisponibilidades = await _context.indisponibilidadMaquinas
                .Include(im => im.idMaquinaNavigation)
                .ToListAsync();

            var indisponibilidadesDto = _mapper.Map<List<IndisponibilidadMaquinasDto>>(indisponibilidades);

            return Ok(indisponibilidadesDto);
        }

        // GET api/<indisponibilidadMaquinasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IndisponibilidadMaquinasDto>> GetIndisponibilidadMaquina(int id)
        {
            var indisponibilidad = await _context.indisponibilidadMaquinas
                .Include(im => im.idMaquinaNavigation)
                .FirstOrDefaultAsync(im => im.id == id);

            if (indisponibilidad == null)
            {
                return NotFound();
            }

            var indisponibilidadDto = _mapper.Map<IndisponibilidadMaquinasDto>(indisponibilidad);

            return Ok(indisponibilidadDto);
        }

        // POST api/<indisponibilidadMaquinasController>
        [HttpPost("post")]
        public async Task<ActionResult<indisponibilidadMaquinas>> PostIndisponibilidadMaquinas(AddIndisponibilidadMaquinasDto addIndisponibilidadMaquinasDto)
        {
            var indisponibilidadMaquinas = _mapper.Map<indisponibilidadMaquinas>(addIndisponibilidadMaquinasDto);

            indisponibilidadMaquinas.createdAt = DateTime.Now;

            _context.indisponibilidadMaquinas.Add(indisponibilidadMaquinas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndisponibilidadMaquina", new { id = indisponibilidadMaquinas.id }, indisponibilidadMaquinas);
        }

        // PUT api/<indisponibilidadMaquinasController>/5
        [HttpPut("put/{id}")]
        public async Task<ActionResult<indisponibilidadMaquinas>> PutIndisponibilidadMaquinas(int id, UpdateIndisponibilidadMaquinasDto updateIndisponibilidadMaquinasDto)
        {
            if (id != updateIndisponibilidadMaquinasDto.id)
            {
                return BadRequest();
            }
            var indisponibilidadMaquinas = await _context.indisponibilidadMaquinas.FindAsync(id);
            if (indisponibilidadMaquinas == null)
            {
                return NotFound();
            }
            _mapper.Map(updateIndisponibilidadMaquinasDto, indisponibilidadMaquinas);
            _context.Entry(indisponibilidadMaquinas).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndisponibilidadMaquinasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(indisponibilidadMaquinas);
        }


        private bool IndisponibilidadMaquinasExists(int id)
        {
            return _context.indisponibilidadMaquinas.Any(e => e.id == id);
        }
    }
}
