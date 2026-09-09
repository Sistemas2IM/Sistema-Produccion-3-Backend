using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.HistorialVencimientoOf.MotivoCambioFechaOf;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf.HistorialVencimientoOf.MotivoCambioFechaOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivoCambioFechaOfController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public MotivoCambioFechaOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<MotivoCambioFechaOfController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MotivoCambioFechaOfDto>>> GetMotivos()
        {
            var motivos = await _context.motivoCambioFechaOf
                .ToListAsync();

            var motivosDto = _mapper.Map<List<MotivoCambioFechaOfDto>>(motivos);

            return Ok(motivosDto);
        }

        // GET api/<motivoCambioFechaOfController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MotivoCambioFechaOfDto>> GetMotivoById(int id)
        {
            var motivo = await _context.motivoCambioFechaOf
                .FirstOrDefaultAsync(m => m.idMotivo == id);

            var motivoDto = _mapper.Map<MotivoCambioFechaOfDto>(motivo);

            if (motivoDto == null)
            {
                return NotFound();
            }

            return Ok(motivoDto);
        }
    }
}
