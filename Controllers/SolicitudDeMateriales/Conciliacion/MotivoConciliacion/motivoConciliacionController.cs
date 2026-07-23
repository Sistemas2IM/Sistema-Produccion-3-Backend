using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.MotivoConciliacion;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales.Conciliacion.MotivoConciliacion
{
    [Route("api/[controller]")]
    [ApiController]
    public class motivoConciliacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public motivoConciliacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<motivoConciliacionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MotivoConciliacionDto>>> GetMotivosConciliacion()
        {
            var motivosConciliacion = await _context.motivoConciliacion
                .ToListAsync();

            var motivosConciliacionDto = _mapper.Map<List<MotivoConciliacionDto>>(motivosConciliacion);

            return Ok(motivosConciliacionDto);
        }

        // GET api/<motivoConciliacionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MotivoConciliacionDto>> GetMotivoConciliacion(int id)
        {
            var motivoConciliacion = await _context.motivoConciliacion
                .FirstOrDefaultAsync(m => m.idMotivo == id);

            if (motivoConciliacion == null)
            {
                return NotFound();
            }

            var motivoConciliacionDto = _mapper.Map<MotivoConciliacionDto>(motivoConciliacion);

            return Ok(motivoConciliacionDto);
        }
    }
}
