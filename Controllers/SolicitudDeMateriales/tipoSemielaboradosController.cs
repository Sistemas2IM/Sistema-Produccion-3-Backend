using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TipoSemielaborados;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoSemielaboradosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoSemielaboradosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<tipoSemielaboradosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TipoSemielaboradosDto>>> GetTipoSemielaborados()
        {
            var tipoSemielaborados = await _context.tipoSemielaborados
                .ToListAsync();

            var tipoSemielaboradosDtos = _mapper.Map<List<TipoSemielaboradosDto>>(tipoSemielaborados);

            return Ok(tipoSemielaboradosDtos);
        }

        // GET api/<tipoSemielaboradosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<TipoSemielaboradosDto>>> GetTipoSemielaboradoId(int id)
        {
            var tipoSemielaborado = await _context.tipoSemielaborados
                .FirstOrDefaultAsync(ts => ts.idSemiElaborado == id);

            var tipoSemielaboradoDto = _mapper.Map<TipoSemielaboradosDto>(tipoSemielaborado);

            if (tipoSemielaboradoDto == null)
            {
                return NotFound($"No se encontraron registros con el ID: {id}");
            }

            return Ok(tipoSemielaboradoDto);
        }

        // POST api/<tipoSemielaboradosController>
        [HttpPost("post")]
        public async Task<ActionResult<tipoSemielaborados>> PostTipoSemielaborado(AddTipoSemielaboradosDto addTipoSemielaboradosDto)
        {
            var tipoSemielaborado = _mapper.Map<tipoSemielaborados>(addTipoSemielaboradosDto);
            _context.tipoSemielaborados.Add(tipoSemielaborado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoSemielaboradoId", new { id = tipoSemielaborado.idSemiElaborado }, tipoSemielaborado);
        }

        // PUT api/<tipoSemielaboradosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTipoSemielaborado(int id, UpdateTipoSemielaboradosDto updateTipoSemielaboradosDto)
        {
            var tipoSemielaborado = await _context.tipoSemielaborados.FindAsync(id);
            if (tipoSemielaborado == null)
            {
                return NotFound($"No se encontraron registros con el ID: {id}");
            }

            _mapper.Map(updateTipoSemielaboradosDto, tipoSemielaborado);
            _context.Entry(tipoSemielaborado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoSemielaboradoExists(id))
                {
                    return NotFound($"No se encontraron registros con el ID: {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTipoSemielaboradosDto);
        }

        private bool TipoSemielaboradoExists(int id)
        {
            return _context.tipoSemielaborados.Any(e => e.idSemiElaborado == id);

        }
    }
}
