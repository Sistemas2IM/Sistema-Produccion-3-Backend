using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ValidacionArranque
{
    [Route("api/[controller]")]
    [ApiController]
    public class validacionArranqueController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public validacionArranqueController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<validacionArranqueController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ValidacionArranqueDto>>> GetValidacionArranque()
        {
            var validacionArranque = await _context.validacionArranque
                .Include(va => va.detalleValidacionArranque)
                .ToListAsync();

            var validacionArranqueDto = _mapper.Map<List<ValidacionArranqueDto>>(validacionArranque);

            return Ok(validacionArranqueDto);
        }
        // GET api/<validacionArranqueController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ValidacionArranqueDto>> GetValidacionArranque(int id)
        {
            var validacionArranque = await _context.validacionArranque
                .Include(va => va.detalleValidacionArranque)
                .FirstAsync(v => v.idValidacionArranque == id);

            if (validacionArranque == null)
            {
                return NotFound();
            }
            var validacionArranqueDto = _mapper.Map<ValidacionArranqueDto>(validacionArranque);

            return Ok(validacionArranqueDto);
        }

        // POST api/<validacionArranqueController>
        [HttpPost("post")]
        public async Task<ActionResult<validacionArranque>> PostValidacionArranque(AddValidacionArranqueDto addValidacionArranqueDto)
        {
            var validacionArranque = _mapper.Map<validacionArranque>(addValidacionArranqueDto);

            _context.validacionArranque.Add(validacionArranque);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetValidacionArranque", new { id = validacionArranque.idValidacionArranque }, validacionArranque);
        }

        // PUT api/<validacionArranqueController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutValidacionArranque(int id, UpdateValidacionArranqueDto updateValidacionArranqueDto)
        {
            var validacionArranque = await _context.validacionArranque.FindAsync(id);
            if (validacionArranque == null)
            {
                return NotFound();
            }

            _mapper.Map(updateValidacionArranqueDto, validacionArranque);
            _context.Entry(validacionArranque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidacionArranqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateValidacionArranqueDto);
        }

        private bool ValidacionArranqueExists(int id)
        {
            return _context.validacionArranque.Any(e => e.idValidacionArranque == id);
        }
    }
}
