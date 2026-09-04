using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.Calidad.CategoriaDefecto;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CategoriaDefecto
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoriaDefectoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public categoriaDefectoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<categoriaDefectoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CategoriaDefectoDto>>> GetCategoriaDefecto()
        {
            var categoriaDefecto = await _context.categoriaDefecto
                .Include(c => c.subtipoDefecto)
                .ToListAsync();

            var categoriaDefectoDto = _mapper.Map<List<CategoriaDefectoDto>>(categoriaDefecto);

            return Ok(categoriaDefectoDto);
        }

        // GET api/<categoriaDefectoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CategoriaDefectoDto>> GetCategoriaDefecto(int id)
        {
            var categoriaDefecto = await _context.categoriaDefecto
                .Include(c => c.subtipoDefecto)
                .FirstOrDefaultAsync(u => u.idCategoria == id);

            if (categoriaDefecto == null)
            {
                return NotFound();
            }

            var categoriaDefectoDto = _mapper.Map<CategoriaDefectoDto>(categoriaDefecto);
            return Ok(categoriaDefectoDto);
        }

        // POST api/<categoriaDefectoController>
        [HttpPost("post")]
        public async Task<ActionResult<categoriaDefecto>> PostCategoriaDefecto(AddCategoriaDefectoDto addCategoriaDefectoDto)
        {
            var categoriaDefecto = _mapper.Map<categoriaDefecto>(addCategoriaDefectoDto);

            _context.categoriaDefecto.Add(categoriaDefecto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoriaDefecto), new { id = categoriaDefecto.idCategoria }, categoriaDefecto);
        }

        // PUT api/<categoriaDefectoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCategoriaDefecto(int id, UpdateCategoriaDefectoDto updateCategoriaDefectoDto)
        {
            var categoriaDefecto = await _context.categoriaDefecto.FindAsync(id);

            if (categoriaDefecto == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCategoriaDefectoDto, categoriaDefecto);
            _context.Entry(categoriaDefecto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaDefectoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCategoriaDefectoDto);
        }

        private bool CategoriaDefectoExists(int id)
        {
            return (_context.categoriaDefecto?.Any(e => e.idCategoria == id)).GetValueOrDefault();
        }
    }
}
