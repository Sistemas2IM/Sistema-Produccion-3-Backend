using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.DictamenCalidad;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.DictamenCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class dictamenCalidadController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public dictamenCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<dictamenCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DictamenCalidadDto>>> GetDictamen()
        {
            var dictamenCalidad = await _context.dictamenCalidad
                .OrderByDescending(d => d.idDictamen)
                .ToListAsync();

            var dictamenCalidadDto = _mapper.Map<List<DictamenCalidadDto>>(dictamenCalidad);

            return Ok(dictamenCalidadDto);
        }

        // GET api/<dictamenCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DictamenCalidadDto>> GetDictamen(int id)
        {
            var dictamenCalidad = await _context.dictamenCalidad.FindAsync(id);

            if (dictamenCalidad == null)
            {
                return NotFound();
            }

            var dictamenCalidadDto = _mapper.Map<DictamenCalidadDto>(dictamenCalidad);

            return Ok(dictamenCalidadDto);
        }    
    }
}
