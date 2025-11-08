using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativos;
using Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativos;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Horarios.TurnosOperativos
{
    [Route("api/[controller]")]
    [ApiController]
    public class turnosOperativosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public turnosOperativosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<turnosOperativosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TurnosOperativosDto>>> GetTurnosOperativos()
        {
            var turnosOperativos = await _context.turnosOperativos
                .ToListAsync();

            var turnosOperativosDto = _mapper.Map<List<TurnosOperativosDto>>(turnosOperativos);

            return Ok(turnosOperativosDto); 
        }

        // GET api/<turnosOperativosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TurnosOperativosDto>> GetTurnosOperativoss(int id)
        {
            var turnoOperativo = await _context.turnosOperativos
            .FirstOrDefaultAsync(t => t.idTurno == id);

            if (turnoOperativo == null)
            {
                return NotFound();
            }
            var turnoOperativoDto = _mapper.Map<TurnosOperativosDto>(turnoOperativo);

            return Ok(turnoOperativoDto);
        }

        // POST api/<turnosOperativosController>
        [HttpPost("post")]
        public async Task<ActionResult<turnosOperativos>> PostTurnosOperativos(AddTurnosOperativosDto addTurnosOperativosDto)
        {
            var turnosOperativos = _mapper.Map<turnosOperativos>(addTurnosOperativosDto);

            _context.turnosOperativos.Add(turnosOperativos);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetTurnosOperativoss", new { id = turnosOperativos.idTurno }, turnosOperativos);
        }

        // PUT api/<turnosOperativosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTurnosOperativoss(int id, UpdateTurnosOperativosDto updateTurnosOperativosDto)
        {
            var turnosOperativos = await _context.turnosOperativos.FindAsync(id);

            if (turnosOperativos == null)
            {
                return NotFound($"No se encontro el Turno Operativo  con el id: {id}");
            }

            _mapper.Map(updateTurnosOperativosDto, turnosOperativos);
            _context.Entry(turnosOperativos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!turnosOperativosExists(id))
                {
                    return NotFound($"No se encontro el Turno Operativo  con el id: {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTurnosOperativosDto);
        }

        private bool turnosOperativosExists(int id)
        {
            return _context.turnosOperativos.Any(e => e.idTurno == id);
        }

    }
}
