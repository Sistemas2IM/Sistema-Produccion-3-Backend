using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.CausaRaizCalidad;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CausaRaizCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class causaRaizCalidadController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public causaRaizCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<causaRaizCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CausaRaizCalidadDto>>> GetCausaRaizCalidad()
        {
            var causaRaizCalidad = await _context.causaRaizCalidad
                .ToListAsync();

            var causaRaizCalidadDto = _mapper.Map<List<CausaRaizCalidadDto>>(causaRaizCalidad);

            return Ok(causaRaizCalidadDto);
        }

        // GET api/<causaRaizCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CausaRaizCalidadDto>> GetCausaRaizCalidad(int id)
        {
            var causaRaizCalidad = await _context.causaRaizCalidad.FindAsync(id);

            if (causaRaizCalidad == null)
            {
                return NotFound();
            }

            var causaRaizCalidadDto = _mapper.Map<CausaRaizCalidadDto>(causaRaizCalidad);

            return Ok(causaRaizCalidadDto);
        }

    }
}
