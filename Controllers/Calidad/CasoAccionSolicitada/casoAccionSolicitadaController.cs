using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoAccionSolicitada;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CasoAccionSolicitada
{
    [Route("api/[controller]")]
    [ApiController]
    public class casoAccionSolicitadaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public casoAccionSolicitadaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<casoAccionSolicitadaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CasoAccionSolicitadaDto>>> GetCasoAccionSolicitada()
        {
            var casoAccionSolicitada = await _context.casoAccionSolicitada
                .Include(t => t.idAccionNavigation)
                .Include(t => t.idEstadoNavigation)
                .ToListAsync();

            var casoAccionSolicitadaDto = _mapper.Map<List<CasoAccionSolicitadaDto>>(casoAccionSolicitada);

            return Ok(casoAccionSolicitadaDto);
        }

        // GET api/<casoAccionSolicitadaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CasoAccionSolicitadaDto>> GetCasoAccionSolicitada(int id)
        {
            var casoAccionSolicitada = await _context.casoAccionSolicitada
                .Include(t => t.idAccionNavigation)
                .Include(t => t.idEstadoNavigation)
                .FirstOrDefaultAsync(u => u.idCasoAccion == id);

            if (casoAccionSolicitada == null)
            {
                return NotFound();
            }

            var casoAccionSolicitadaDto = _mapper.Map<CasoAccionSolicitadaDto>(casoAccionSolicitada);
            return Ok(casoAccionSolicitadaDto);
        }

        // POST api/<casoAccionSolicitadaController>
        [HttpPost("post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<casoAccionSolicitadaCreateResponseDTO>> PostCasoAccion(AddCasoAccionSolicitadaDto addCasoAccionSolicitadaDto)
        {
            var casoAccionSolicitada = _mapper.Map<casoAccionSolicitada>(addCasoAccionSolicitadaDto);

            _context.casoAccionSolicitada.Add(casoAccionSolicitada);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCasoAccionSolicitada), 
                new { id = casoAccionSolicitada.idCasoAccion }, 
                new casoAccionSolicitadaCreateResponseDTO
                {
                    idCasoAccion = casoAccionSolicitada.idCasoAccion,
                    Message = "Se ha creado correctamente el registro de casoAccionSolicitada."
                });
        }

        // PUT api/<casoAccionSolicitadaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCasoAccion(int id, UpdateCasoAccionSolicitadaDto updateCasoAccionSolicitadaDto)
        {
            var casoAccionSolicitada = await _context.casoAccionSolicitada.FindAsync(id);

            if (casoAccionSolicitada == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCasoAccionSolicitadaDto, casoAccionSolicitada);
            _context.Entry(casoAccionSolicitada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasoAccionSolicitadaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCasoAccionSolicitadaDto);
        }

        private bool CasoAccionSolicitadaExists(int id)
        {
            return (_context.casoAccionSolicitada?.Any(e => e.idCasoAccion == id)).GetValueOrDefault();
        }
    }
}
