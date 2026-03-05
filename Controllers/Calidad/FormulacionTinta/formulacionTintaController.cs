using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FormulacionTinta
{
    [Route("api/[controller]")]
    [ApiController]
    public class formulacionTintaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public formulacionTintaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<formulacionTintaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FormulacionTintaDto>>> GetFormulacion()
        {
            var formulacion = await _context.formulacionTinta.ToListAsync();

            var formulacionDto = _mapper.Map<List<FormulacionTintaDto>>(formulacion);

            return Ok(formulacionDto);
        }

        // GET api/<formulacionTintaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FormulacionTintaDto>> GetFormulacion(int id)
        {
            var formulacion = await _context.formulacionTinta.FirstOrDefaultAsync(f => f.idFormulacion == id);

            if (formulacion == null)
            {
                return NotFound();
            }
            var formulacionDto = _mapper.Map<FormulacionTintaDto>(formulacion);

            return Ok(formulacionDto);

        }

        // POST api/<formulacionTintaController>
        [HttpPost("post")]
        public async Task<ActionResult<formulacionTinta>> PostFormulacion(AddFormulacionTintaDto addFormulacionTintaDto)
        {
            var formulacion = _mapper.Map<formulacionTinta>(addFormulacionTintaDto);

            _context.formulacionTinta.Add(formulacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormulacion), new { id = formulacion.idFormulacion }, formulacion);
        }

        // PUT api/<formulacionTintaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFormulacion(int id, UpdateFormulacionTintaDto updateFormulacionTintaDto)
        {
            var formulacion = await _context.formulacionTinta.FindAsync(id);

            if (formulacion == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFormulacionTintaDto, formulacion);
            _context.Entry(formulacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormulacionTintaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFormulacionTintaDto);
        }

        private bool FormulacionTintaExists(int id)
        {
            return _context.formulacionTinta.Any(e => e.idFormulacion == id);
        }
    }
}
