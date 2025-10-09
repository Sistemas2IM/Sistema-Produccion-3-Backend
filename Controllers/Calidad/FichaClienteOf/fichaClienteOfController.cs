using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaClienteOf;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaClienteOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class fichaClienteOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public fichaClienteOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<fichaClienteOfController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FichaClienteOfDto>>> GetFichaClienteOf()
        {
            var fichaClienteOf = await _context.fichaClienteOf.ToListAsync();

            var fichaClienteOfDto = _mapper.Map<List<FichaClienteOfDto>>(fichaClienteOf);

            return Ok(fichaClienteOfDto);
        }

        // GET api/<fichaClienteOfController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FichaClienteOfDto>> GetFichaClienteOf(int id)
        {
            var fichaClienteOf = await _context.fichaClienteOf.FirstOrDefaultAsync(f => f.idFichaCliente == id);

            if (fichaClienteOf == null)
            {
                return NotFound();
            }

            var fichaClienteOfDto = _mapper.Map<FichaClienteOfDto>(fichaClienteOf);

            return Ok(fichaClienteOfDto);
        }

        // POST api/<fichaClienteOfController>
        [HttpPost("post")]
        public async Task<ActionResult<fichaClienteOf>> PostFichaClienteOf(AddFichaClienteOfDto addFichaClienteOfDto)
        {
            var fichaClienteOf = _mapper.Map<fichaClienteOf>(addFichaClienteOfDto);

            _context.fichaClienteOf.Add(fichaClienteOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFichaClienteOf", new { id = fichaClienteOf.idFichaCliente }, fichaClienteOf);
        }

        // PUT api/<fichaClienteOfController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFichaClienteOf(int id, UpdateFichaClienteOfDto updateFichaClienteOfDto)
        {
            var fichaClienteOf = await _context.fichaClienteOf.FindAsync(id);

            if (fichaClienteOf == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFichaClienteOfDto, fichaClienteOf);
            _context.Entry(fichaClienteOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaClienteOfExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFichaClienteOfDto);
        }

        private bool FichaClienteOfExists(int id)
        {
            return _context.fichaClienteOf.Any(e => e.idFichaCliente == id);
        }
    }
}
