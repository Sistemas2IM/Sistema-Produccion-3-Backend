using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.RegistroLamparas
{
    [Route("api/[controller]")]
    [ApiController]
    public class registroLamparasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public registroLamparasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<registroLamparasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<RegistroLamparasDto>>> GetRegistroLamparas()
        {
            var registroLamparas = await _context.registroLamparas.ToListAsync();

            var registroLamparasDto = _mapper.Map<List<RegistroLamparasDto>>(registroLamparas);

            return Ok(registroLamparasDto);
        }

        // GET api/<registroLamparasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<RegistroLamparasDto>> GetRegistroLamparas(int id)
        {
            var registroLamparas = await _context.registroLamparas.FindAsync(id);

            if (registroLamparas == null)
            {
                return NotFound();
            }

            var registroLamparasDto = _mapper.Map<RegistroLamparasDto>(registroLamparas);

            return Ok(registroLamparasDto);
        }

        // POST api/<registroLamparasController>
        [HttpPost("post")]
        public async Task<ActionResult> PostRegistroLamparas(AddRegistroLamparasDto addRegistroLamparasDto)
        {
            var registroLamparas = _mapper.Map<registroLamparas>(addRegistroLamparasDto);

            _context.registroLamparas.Add(registroLamparas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroLamparas", new { id = registroLamparas.idRegistroLampara }, addRegistroLamparasDto);
        }

        // PUT api/<registroLamparasController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutRegistroLamparas(int id, UpdateRegistroLamparasDto updateRegistroLamparasDto)
        {
            var registroLamparas = await _context.registroLamparas.FindAsync(id);

            if (registroLamparas == null)
            {
                return NotFound();
            }

            _mapper.Map(updateRegistroLamparasDto, registroLamparas);
            _context.Entry(registroLamparas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroLamparasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool RegistroLamparasExists(int id)
        {
            return _context.registroLamparas.Any(e => e.idRegistroLampara == id);
        }
    }
}
