using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.AccionSolicitada;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.AccionSolicitada
{
    [Route("api/[controller]")]
    [ApiController]
    public class accionSolicitadaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public accionSolicitadaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<accionSolicitadaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AccionSolicitadaDto>>> GetAccionSolicitada()
        {
            var accionSolicitada = await _context.accionSolicitada
                .ToListAsync();

            var accionSolicitadaDto = _mapper.Map<List<AccionSolicitadaDto>>(accionSolicitada);

            return Ok(accionSolicitadaDto);
        }

        // GET api/<accionSolicitadaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<AccionSolicitadaDto>> GetAccionSolicitada(int id)
        {
            var accionSolicitada = await _context.accionSolicitada.FindAsync(id);

            if (accionSolicitada == null)
            {
                return NotFound();
            }

            var accionSolicitadaDto = _mapper.Map<AccionSolicitadaDto>(accionSolicitada);

            return Ok(accionSolicitadaDto);
        }

        // POST api/<accionSolicitadaController>
        [HttpPost("post")]
        public async Task<ActionResult<accionSolicitada>> PostAccionSolicitada(AddAccionSolicitadaDto addAccionSolicitadaDto)
        {
            var accionSolicitada = _mapper.Map<accionSolicitada>(addAccionSolicitadaDto);

            _context.accionSolicitada.Add(accionSolicitada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccionSolicitada", new { id = accionSolicitada.idAccion }, accionSolicitada);
        }

        // PUT api/<accionSolicitadaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutAccionSolicitada(int id, UpdateAccionSolicitadaDto updateAccionSolicitadaDto)
        {
            var accionSolicitada = await _context.accionSolicitada.FindAsync(id);

            if (accionSolicitada == null)
            {
                return NotFound();
            }

            _mapper.Map(updateAccionSolicitadaDto, accionSolicitada);
            _context.Entry(accionSolicitada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccionSolicitadaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateAccionSolicitadaDto);
        }

        private bool AccionSolicitadaExists(int id)
        {
            return _context.accionSolicitada.Any(e => e.idAccion == id);
        }
    }
}
