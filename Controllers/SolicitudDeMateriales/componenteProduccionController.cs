using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class componenteProduccionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public componenteProduccionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<componenteProduccionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ComponenteProduccionDto>>> etComponenteProduccion()
        {
            var componentes = await Task.FromResult(_context.componenteProduccion.ToList());

            var componentesDto = _mapper.Map<List<ComponenteProduccionDto>>(componentes);

            return Ok(componentesDto);
        }

        // GET api/<componenteProduccionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<ComponenteProduccionDto>>> GetComponenteProduccionId(int id)
        {
            var componente = await _context.componenteProduccion
                .FindAsync(id);

            var componenteDto = _mapper.Map<ComponenteProduccionDto>(componente);

            if (componenteDto == null)
            {
                return NotFound();
            }

            return Ok(componenteDto);
        }

        // POST api/<componenteProduccionController>
        [HttpPost("post")]
        public async Task<ActionResult<componenteProduccion>> PostComponente(AddComponenteProduccionDto componenteDto)
        {
            var componente = _mapper.Map<componenteProduccion>(componenteDto);
            _context.componenteProduccion.Add(componente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponenteProduccionId", new { id = componente.idComponente }, componente);
        }

        // PUT api/<componenteProduccionController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutComponente(int id, UpdateComponenteProduccionDto componenteDto)
        {
            var componente = await _context.componenteProduccion.FindAsync(id);
            if (componente == null)
            {
                return NotFound();
            }

            _mapper.Map(componenteDto, componente);
            _context.Entry(componente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponenteProduccionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(componenteDto);
        }

        private bool ComponenteProduccionExists(int id)
        {
            return _context.componenteProduccion.Any(e => e.idComponente == id);
        }
    }
}
