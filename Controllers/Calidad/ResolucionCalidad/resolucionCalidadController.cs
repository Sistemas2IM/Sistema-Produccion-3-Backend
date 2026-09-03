using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.ResolucionCalidad;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.ResolucionCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class resolucionCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public resolucionCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<resolucionCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ResolucionCalidadDto>>> GetResolucionCalidad()
        {
            var resolucionCalidad = await _context.resolucionCalidad
                .ToListAsync();

            var resolucionCalidadDto = _mapper.Map<List<ResolucionCalidadDto>>(resolucionCalidad);

            return Ok(resolucionCalidadDto);
        }

        // GET api/<resolucionCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ResolucionCalidadDto>> GetResolucionCalidad(int id)
        {
            var resolucionCalidad = await _context.resolucionCalidad.FindAsync(id);

            if (resolucionCalidad == null)
            {
                return NotFound();
            }

            var resolucionCalidadDto = _mapper.Map<ResolucionCalidadDto>(resolucionCalidad);

            return Ok(resolucionCalidadDto);
        }
    }
}
