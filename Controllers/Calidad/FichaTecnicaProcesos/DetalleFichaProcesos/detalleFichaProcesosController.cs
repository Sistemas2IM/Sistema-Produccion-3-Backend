using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleFichaProcesosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public DetalleFichaProcesosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<DetalleFichaProcesosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleFichaProcesosDto>>> GetDetalleProcesos()
        {
            var detalleProcesos = await _context.detalleFichaProcesos.ToListAsync();

            var detalleProcesosDto = _mapper.Map<List<DetalleFichaProcesosDto>>(detalleProcesos);

            return Ok(detalleProcesosDto);
        }

        // GET api/<DetalleFichaProcesosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleFichaProcesosDto>> GetDetalleProcesos(int id)
        {
            var detalleProcesos = await _context.detalleFichaProcesos.FindAsync(id);

            if (detalleProcesos == null)
            {
                return NotFound();
            }
            var detalleProcesosDto = _mapper.Map<DetalleFichaProcesosDto>(detalleProcesos);

            return Ok(detalleProcesosDto);
        }
        // POST api/<DetalleFichaProcesosController>
        [HttpPost("post")]
        public async Task<ActionResult<detalleFichaProcesos>> PostDetalleProcesos(AddDetalleFichaProcesosDto addDetalleFichaProcesosDto)
        {
            var detalleProcesos = _mapper.Map<detalleFichaProcesos>(addDetalleFichaProcesosDto);

            _context.detalleFichaProcesos.Add(detalleProcesos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleProcesos", new { id = detalleProcesos.idDetalle }, detalleProcesos);
        }
        // PUT api/<DetalleFichaProcesosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutDetalleProceso(int id, UpdateDetalleFichaProcesosDto updateDetalleFichaProcesosDto)
        {
            var detalleProcesos = await _context.detalleFichaProcesos.FindAsync(id);
            if (detalleProcesos == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDetalleFichaProcesosDto, detalleProcesos);
            _context.Entry(detalleProcesos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                // Aquí puedes manejar la excepción de la forma que consideres adecuada
                return StatusCode(500, $"Error al actualizar el detalle del proceso: {ex.Message}");                    
                                
            }

    }
}
