using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.SecuenciaColor
{
    [Route("api/[controller]")]
    [ApiController]
    public class secuenciaColorController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public secuenciaColorController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<secuenciaColorController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SecuenciaColorDto>>> GetSecuenciaColor()
        {
            var secuenciaColor = await _context.secuenciaColor.ToListAsync();

            var secuenciaColorDto = _mapper.Map<List<SecuenciaColorDto>>(secuenciaColor);

            return Ok(secuenciaColorDto);
        }

        // GET api/<secuenciaColorController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SecuenciaColorDto>> GetSecuenciaColor(int id)
        {
            var secuenciaColor = await _context.secuenciaColor.FindAsync(id);

            if (secuenciaColor == null)
            {
                return NotFound();
            }
            var secuenciaColorDto = _mapper.Map<SecuenciaColorDto>(secuenciaColor);

            return Ok(secuenciaColorDto);
        }

        // POST api/<secuenciaColorController>
        [HttpPost("post")]
        public async Task<ActionResult<secuenciaColor>> PostSecuenciaColor(AddSecuenciaColorDto addSecuenciaColorDto)
        {
            var secuenciaColor = _mapper.Map<secuenciaColor>(addSecuenciaColorDto);

            _context.secuenciaColor.Add(secuenciaColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSecuenciaColor", new { id = secuenciaColor.idSecuenciaColor }, secuenciaColor);
        }

        // PUT api/<secuenciaColorController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutSecuenciaSolor(int id, UpdateSecuenciaColorDto updateSecuenciaColorDto)
        {
            var secuenciaColor = await _context.secuenciaColor.FindAsync(id);
            if (secuenciaColor == null)
            {
                return NotFound();
            }

            _mapper.Map(updateSecuenciaColorDto, secuenciaColor);
            _context.Entry(secuenciaColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecuenciaColorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateSecuenciaColorDto);
        }

        private bool SecuenciaColorExists(int id)
        {
            return _context.secuenciaColor.Any(e => e.idSecuenciaColor == id);
        }
    }
}
