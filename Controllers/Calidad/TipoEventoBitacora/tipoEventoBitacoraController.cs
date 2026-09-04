using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.TipoEventoBitacora;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.TipoEventoBitacora
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoEventoBitacoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoEventoBitacoraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<tipoEventoBitacoraController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TipoEventoBitacoraDto>>> GetTipoEventoBitacora()
        {
            var tipoEvento = await _context.tipoEventoBitacora
                .ToListAsync();

            var tipoEventoDto = _mapper.Map<IEnumerable<TipoEventoBitacoraDto>>(tipoEvento);

            return Ok(tipoEventoDto);
        }

        // GET api/<tipoEventoBitacoraController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TipoEventoBitacoraDto>> GetTipoEventoBitacora(int id)
        {
            var tipoEvento = await _context.tipoEventoBitacora.FindAsync(id);

            if (tipoEvento == null)
            {
                return NotFound();
            }

            var tipoEventoDto = _mapper.Map<TipoEventoBitacoraDto>(tipoEvento);
            return Ok(tipoEventoDto);
        }

        // POST api/<tipoEventoBitacoraController>
        [HttpPost("post")]
        public async Task<ActionResult<tipoEventoBitacora>> PostTipoEvento(AddTipoEventoBitacoraDto addTipoEventoBitacoraDto)
        {
            var tipoEvento = _mapper.Map<tipoEventoBitacora>(addTipoEventoBitacoraDto);

            _context.tipoEventoBitacora.Add(tipoEvento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoEventoBitacora), new { id = tipoEvento.idTipoEvento }, tipoEvento);
        }

        // PUT api/<tipoEventoBitacoraController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTipoEvento(int id, UpdateTipoEventoBitacoraDto updateTipoEventoBitacoraDto)
        {
            var tipoEvento = await _context.tipoEventoBitacora.FindAsync(id);

            if (tipoEvento == null)
            {
                return NotFound();
            }

            _mapper.Map(updateTipoEventoBitacoraDto, tipoEvento);
            _context.Entry(tipoEvento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEventoBitacoraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTipoEventoBitacoraDto);
        }

        private bool TipoEventoBitacoraExists(int id)
        {
            return (_context.tipoEventoBitacora?.Any(e => e.idTipoEvento == id)).GetValueOrDefault();
        }
    }
}
