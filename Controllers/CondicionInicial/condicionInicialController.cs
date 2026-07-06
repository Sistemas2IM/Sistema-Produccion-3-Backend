using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.CondicionInicial
{
    [Route("api/[controller]")]
    [ApiController]
    public class condicionInicialController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public condicionInicialController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<condicionInicialController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CondicionInicialDto>>> GetCondicionInicial()
        {
            var condicionInicial = await _context.condicionInicial
                .Include(ci => ci.detalleCondicionInicial)
                .ToListAsync();

            var condicionInicialDto = _mapper.Map<List<CondicionInicialDto>>(condicionInicial);

            return Ok(condicionInicialDto);
        }

        // GET api/<condicionInicialController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CondicionInicialDto>> GetCondicionInicial(int id)
        {
            var condicionInicial = await _context.condicionInicial
                .Include(ci => ci.detalleCondicionInicial)
                .FirstOrDefaultAsync(ci => ci.idCondicionInicial == id);

            if (condicionInicial == null)
            {
                return NotFound();
            }
            var condicionInicialDto = _mapper.Map<CondicionInicialDto>(condicionInicial);

            return Ok(condicionInicialDto);
        }

        // get por idProceso
        [HttpGet("get/proceso/{idProceso}")]
        public async Task<ActionResult<IEnumerable<CondicionInicialDto>>> GetCondicionInicialProceso(int idProceso)
        {
            var condicionInicial = await _context.condicionInicial
                .Include(ci => ci.detalleCondicionInicial)
                .Where(ci => ci.idProceso == idProceso)
                .ToListAsync();

            var condicionInicialDto = _mapper.Map<List<CondicionInicialDto>>(condicionInicial);

            return Ok(condicionInicialDto);
        }

        // POST api/<condicionInicialController>
        [HttpPost("post")]
        public async Task<ActionResult<condicionInicial>> PostCondicionInicial(AddCondicionInicialDto condicionInicialDto)
        {
            var condicionInicial = _mapper.Map<condicionInicial>(condicionInicialDto);
            _context.condicionInicial.Add(condicionInicial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCondicionInicial", new { id = condicionInicial.idCondicionInicial }, condicionInicial);
        }

        // PUT api/<condicionInicialController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCondicionInicial(int id, UpdateCondicionInicialDto updateCondicionInicialDto)
        {
            var condicionInicial = await _context.condicionInicial.FindAsync(id);
            if (condicionInicial == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCondicionInicialDto, condicionInicial);
            _context.Entry(condicionInicial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CondicionInicialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCondicionInicialDto);
        }

        private bool CondicionInicialExists(int id)
        {
            return _context.condicionInicial.Any(e => e.idCondicionInicial == id);
        }
    }
}
