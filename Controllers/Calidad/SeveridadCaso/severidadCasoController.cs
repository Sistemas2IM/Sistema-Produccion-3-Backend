using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.SeveridadCaso;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.SeveridadCaso
{
    [Route("api/[controller]")]
    [ApiController]
    public class severidadCasoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public severidadCasoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<severidadCasoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SeveridadCasoDto>>> GetSeveridadCaso()
        {
            var severidadCaso = await _context.severidadCaso
                .ToListAsync();

            var severidadCasoDto = _mapper.Map<List<SeveridadCasoDto>>(severidadCaso);

            return Ok(severidadCasoDto);
        }

        // GET api/<severidadCasoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SeveridadCasoDto>> GetSeveridadCaso(int id)
        {
            var severidadCaso = await _context.severidadCaso
                .FirstOrDefaultAsync(u => u.idSeveridad == id);

            if (severidadCaso == null)
            {
                return NotFound();
            }
            var severidadCasoDto = _mapper.Map<SeveridadCasoDto>(severidadCaso);

            return Ok(severidadCasoDto);
        }

        // POST api/<severidadCasoController>
        [HttpPost("post")]
        public async Task<ActionResult<severidadCaso>> PostSeverdiadCaso(AddSeveridadCasoDto addSeveridadCasoDto)
        {
            var severidadCaso = _mapper.Map<severidadCaso>(addSeveridadCasoDto);

            _context.severidadCaso.Add(severidadCaso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeveridadCaso", new { id = severidadCaso.idSeveridad }, severidadCaso);
        }

        // PUT api/<severidadCasoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutSeveridadCaso(int id, UpdateSeveridadCasoDto updateSeveridadCasoDto)
        {
            var severidadCaso = await _context.severidadCaso.FindAsync(id);

            if (severidadCaso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateSeveridadCasoDto, severidadCaso);
            _context.Entry(severidadCaso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeveridadCasoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateSeveridadCasoDto);
        }

        private bool SeveridadCasoExists(int id)
        {
            return _context.severidadCaso.Any(e => e.idSeveridad == id);
        }
    }
}
