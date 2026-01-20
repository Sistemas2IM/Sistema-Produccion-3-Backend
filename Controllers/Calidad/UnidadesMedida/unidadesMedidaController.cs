using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.UnidadesMedida;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.UnidadesMedida
{
    [Route("api/[controller]")]
    [ApiController]
    public class unidadesMedidaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public unidadesMedidaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<unidadesMedidaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<UnidadesMedidaDto>>> GetUnidadesMedida()
        {
            var unidadesMedida = await _context.unidadesMedida
                .ToListAsync();

            var unidadesMedidaDto = _mapper.Map<List<UnidadesMedidaDto>>(unidadesMedida);

            return Ok(unidadesMedidaDto);
        }

        // GET api/<unidadesMedidaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<UnidadesMedidaDto>> GetUnidadesMedida(int id)
        {
            var unidadesMedida = await _context.unidadesMedida
                .FirstOrDefaultAsync(u => u.idUnidad == id);

            if (unidadesMedida == null)
            {
                return NotFound();
            }

            var unidadesMedidaDto = _mapper.Map<UnidadesMedidaDto>(unidadesMedida);

            return Ok(unidadesMedidaDto);
        }

        // POR CAMPO "TIPO
        [HttpGet("get/tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<UnidadesMedidaDto>>> GetUnidadesMedidaTipo(string tipo)
        {
            var unidadesMedida = await _context.unidadesMedida
                .Where(t => t.tipo == tipo)
                .ToListAsync();

            var unidadesMedidaDto = _mapper.Map<List<UnidadesMedidaDto>>(unidadesMedida);

            return Ok(unidadesMedidaDto);
        }

        // POST api/<unidadesMedidaController>
        [HttpPost("post")]
        public async Task<ActionResult<unidadesMedida>> PostUnidadMedida(AddUnidadesMedidaDto addUnidadesMedidaDto)
        {
            var unidadMedida = _mapper.Map<unidadesMedida>(addUnidadesMedidaDto);

            _context.unidadesMedida.Add(unidadMedida);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidadesMedida", new { id = unidadMedida.idUnidad }, unidadMedida);
        }

        // PUT api/<unidadesMedidaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutUnidadMedida(int id, UpdateUnidadesMedidaDto updateUnidadesMedidaDto)
        {
            var unidadMedida = await _context.unidadesMedida.FindAsync(id);
            if (unidadMedida == null)
            {
                return NotFound();
            }

            _mapper.Map(updateUnidadesMedidaDto, unidadMedida);
            _context.Entry(unidadMedida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadMedidaExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateUnidadesMedidaDto);
        }

        private bool UnidadMedidaExists(int id)
        {
            return _context.unidadesMedida.Any(e => e.idUnidad == id);
        }
    }
}
