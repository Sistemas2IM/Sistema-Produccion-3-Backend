using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TipoSemielaborados;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoComponenteController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoComponenteController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<tipoComponenteController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TipoComponenteDto>>> GettipoComponente()
        {
            var tipoComponente = await _context.tipoComponente
                .ToListAsync();

            var tipoComponenteDtos = _mapper.Map<List<TipoComponenteDto>>(tipoComponente);

            return Ok(tipoComponenteDtos);
        }

        // GET api/<tipoComponenteController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<TipoComponenteDto>>> GetTipoSemielaboradoId(int id)
        {
            var tipoSemielaborado = await _context.tipoComponente
                .FirstOrDefaultAsync(ts => ts.idTipoComponente == id);

            var tipoSemielaboradoDto = _mapper.Map<TipoComponenteDto>(tipoSemielaborado);

            if (tipoSemielaboradoDto == null)
            {
                return NotFound($"No se encontraron registros con el ID: {id}");
            }

            return Ok(tipoSemielaboradoDto);
        }

        // POST api/<tipoComponenteController>
        [HttpPost("post")]
        public async Task<ActionResult<tipoComponente>> PostTipoSemielaborado(AddTipoComponenteDto addtipoComponenteDto)
        {
            var tipoSemielaborado = _mapper.Map<tipoComponente>(addtipoComponenteDto);
            _context.tipoComponente.Add(tipoSemielaborado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoSemielaboradoId", new { id = tipoSemielaborado.idTipoComponente }, tipoSemielaborado);
        }

        // PUT api/<tipoComponenteController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTipoSemielaborado(int id, UpdateTipoComponenteDto updatetipoComponenteDto)
        {
            var tipoSemielaborado = await _context.tipoComponente.FindAsync(id);
            if (tipoSemielaborado == null)
            {
                return NotFound($"No se encontraron registros con el ID: {id}");
            }

            _mapper.Map(updatetipoComponenteDto, tipoSemielaborado);
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

            return Ok(updatetipoComponenteDto);
        }

        private bool TipoSemielaboradoExists(int id)
        {
            return _context.tipoComponente.Any(e => e.idTipoComponente == id);

        }
    }
}
