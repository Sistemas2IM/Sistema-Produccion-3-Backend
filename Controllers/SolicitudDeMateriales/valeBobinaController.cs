using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class valeBobinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public valeBobinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<valeBobinaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ValeBobinaDto>>> GetValeBobina()
        {
            var valeBobinas = await _context.valeBobina
                .Include(vb => vb.idMaterialNavigation)
                .ToListAsync();

            var valeBobinasDto = _mapper.Map<List<ValeBobinaDto>>(valeBobinas);

            return Ok(valeBobinasDto);
        }


        // GET api/<valeBobinaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ValeBobinaDto>> GetValeBobina(int id)
        { 
            var valeBobina = await _context.valeBobina
                .Include(vb => vb.idMaterialNavigation)
                .FirstOrDefaultAsync(vb => vb.idVale == id);

            var valeBobinaDto = _mapper.Map<ValeBobinaDto>(valeBobina);

            if (valeBobina == null)
            {
                return NotFound();
            }

            return Ok(valeBobinaDto);
        }

        // POST api/<valeBobinaController>
        [HttpPost("post")]
        public async Task<ActionResult<valeBobina>> PostValeBobina(AddValeBobinaDto addValeBobinaDto)
        {
            var valeBobina = _mapper.Map<valeBobina>(addValeBobinaDto);
            _context.valeBobina.Add(valeBobina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValeBobina", new { id = valeBobina.idVale }, valeBobina);
        }

        // PUT api/<valeBobinaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutValeBobina(int id, UpdateValeBobinaDto updateValeBobinaDto)
        {
            var valeBobina = await _context.valeBobina.FindAsync(id);
            if (valeBobina == null)
            {
                return NotFound();
            }

            _mapper.Map(updateValeBobinaDto, valeBobina);
            _context.Entry(valeBobina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!valeBobinaExists(id))
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

        private bool valeBobinaExists(int id)
        {
            return _context.valeBobina.Any(e => e.idVale == id);
        }
    }
}
