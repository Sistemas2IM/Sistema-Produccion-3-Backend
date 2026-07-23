using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.SubtipoDefecto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.SubtipoDefecto
{
    [Route("api/[controller]")]
    [ApiController]
    public class subtipoDefectoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public subtipoDefectoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<subtipoDefectoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SubtipoDefectoDto>>> GetSubtipoDefecto()
        {
            var subtipoDefecto = await _context.subtipoDefecto
                .ToListAsync();

            var subtipoDefectoDto = _mapper.Map<List<SubtipoDefectoDto>>(subtipoDefecto);

            return Ok(subtipoDefectoDto);
        }

        // GET api/<subtipoDefectoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SubtipoDefectoDto>> GetSubtipoDefecto(int id)
        {
            var subtipoDefecto = await _context.subtipoDefecto
                .FirstOrDefaultAsync(u => u.idSubtipo == id);

            if (subtipoDefecto == null)
            {
                return NotFound();
            }

            var subtipoDefectoDto = _mapper.Map<SubtipoDefectoDto>(subtipoDefecto);

            return Ok(subtipoDefectoDto);
        }

        // GET por categoria
        [HttpGet("get/categoria/{idCategoria}")]
        public async Task<ActionResult<IEnumerable<SubtipoDefectoDto>>> GetSubtipoDefectoPorCategoria(int idCategoria)
        {
            var subtipoDefecto = await _context.subtipoDefecto
                .Where(u => u.idCategoria == idCategoria)
                .ToListAsync();

            var subtipoDefectoDto = _mapper.Map<List<SubtipoDefectoDto>>(subtipoDefecto);
            return Ok(subtipoDefectoDto);
        }

        // POST api/<subtipoDefectoController>
        [HttpPost("post")]
        public async Task<ActionResult<subtipoDefecto>> PostSubtipoDefecto(AddSubtipoDefectoDto addSubtipoDefectoDto)
        {
            var subtipoDefecto = _mapper.Map<subtipoDefecto>(addSubtipoDefectoDto);

            _context.subtipoDefecto.Add(subtipoDefecto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubtipoDefecto", new { id = subtipoDefecto.idSubtipo }, subtipoDefecto);
        }

        // PUT api/<subtipoDefectoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutSubtipoDefecto(int id, UpdateSubtipoDefectoDto updateSubtipoDefectoDto)
        {
            var subtipoDefecto = await _context.subtipoDefecto.FindAsync(id);

            if (subtipoDefecto == null)
            {
                return NotFound();
            }

            _mapper.Map(updateSubtipoDefectoDto, subtipoDefecto);
            _context.Entry(subtipoDefecto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubtipoDefectoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateSubtipoDefectoDto);
        }

        private bool SubtipoDefectoExists(int id)
        {
            return _context.subtipoDefecto.Any(e => e.idSubtipo == id);
        }
    }
}
