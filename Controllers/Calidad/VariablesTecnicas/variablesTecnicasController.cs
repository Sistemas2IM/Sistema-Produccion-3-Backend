using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariablesTecnicas;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.VariablesTecnicas
{
    [Route("api/[controller]")]
    [ApiController]
    public class variablesTecnicasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public variablesTecnicasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<variablesTecnicasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<VariablesTecnicasDto>>> GetVariablesTecnicas()
        {
            var variablesTecnicas = await _context.variablesTecnicas
                .ToListAsync();

            var variablesTecnicasDto = _mapper.Map<List<VariablesTecnicasDto>>(variablesTecnicas);

            return Ok(variablesTecnicasDto);
        }

        // GET api/<variablesTecnicasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<VariablesTecnicasDto>> GetVariablesTecnicas(int id)
        {
            var variablesTecnicas = await _context.variablesTecnicas
                .FirstOrDefaultAsync(u => u.idVariable == id);

            if (variablesTecnicas == null)
            {
                return NotFound();
            }

            var variablesTecnicasDto = _mapper.Map<VariablesTecnicasDto>(variablesTecnicas);
            return Ok(variablesTecnicasDto);
        }

        // POST api/<variablesTecnicasController>
        [HttpPost("post")]
        public async Task<ActionResult<variablesTecnicas>> PostVariablesTecnicas(AddVariablesTecnicasDto addVariablesTecnicasDto)
        {
            var variablesTecnicas = _mapper.Map<variablesTecnicas>(addVariablesTecnicasDto);

            _context.variablesTecnicas.Add(variablesTecnicas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVariablesTecnicas", new { id = variablesTecnicas.idVariable }, variablesTecnicas);
        }

        // PUT api/<variablesTecnicasController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutVariablesTecnicas(int id, UpdateVariablesTecnicasDto updateVariablesTecnicasDto)
        {
            var variablesTecnicas = await _context.variablesTecnicas.FindAsync(id);

            if (variablesTecnicas == null)
            {
                return NotFound();
            }

            _mapper.Map(updateVariablesTecnicasDto, variablesTecnicas);
            _context.Entry(variablesTecnicas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!variablesTecnicasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateVariablesTecnicasDto);
        }

        private bool variablesTecnicasExists(int id)
        {
            return _context.variablesTecnicas.Any(e => e.idVariable == id);
        }
    }
}
