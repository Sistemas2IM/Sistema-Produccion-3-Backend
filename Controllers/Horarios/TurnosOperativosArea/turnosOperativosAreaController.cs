using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativosArea;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Horarios.TurnosOperativosArea
{
    [Route("api/[controller]")]
    [ApiController]
    public class turnosOperativosAreasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public turnosOperativosAreasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<turnosOperativosAreaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TurnosOperativosAreaDto>>> GetTurnosOperativos()
        {
            var turnosOperativosArea = await _context.turnosOperativosArea
                .ToListAsync();

            var turnosOperativosAreaAreaDto = _mapper.Map<List<TurnosOperativosAreaDto>>(turnosOperativosArea);

            return Ok(turnosOperativosAreaAreaDto);
        }

        // GET api/<turnosOperativosAreaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TurnosOperativosAreaDto>> GetTurnosOperativos(int id)
        {
            var turnoOperativo = await _context.turnosOperativosArea
                .FirstOrDefaultAsync(t => t.idTurno == id);

            if (turnoOperativo == null)
            {
                return NotFound();
            }

            var turnoOperativoDto = _mapper.Map<TurnosOperativosAreaDto>(turnoOperativo);

            return Ok(turnoOperativoDto);
        }

        // GET api/<turnosOperativosAreaController>/5
        [HttpGet("get/area/{idArea}")]
        public async Task<ActionResult<TurnosOperativosAreaDto>> GetTurnosOperativosArea(int idArea)
        {
            // 1. Preparamos la consulta base (sin filtros de área todavía)
            var query = _context.turnosOperativosArea
                .Include(t => t.idTurnoNavigation)
                .AsQueryable();

            // 2. Aplicamos el filtro SOLO si NO es admin (17)
            if (idArea != 17)
            {
                query = query.Where(t => t.idArea == idArea);
            }

            // 3. Ejecutamos
            var lista = await query.ToListAsync();
            var dtos = _mapper.Map<List<TurnosOperativosAreaDto>>(lista);

            return Ok(dtos);
        }

        // POST api/<turnosOperativosAreaController>
        [HttpPost("post")]
        public async Task<ActionResult<turnosOperativosArea>> PostTurnosOperativos(AddTurnosOperativosAreaDto addTurnosOperativosAreaDto)
        {
            var turnoOperativo = _mapper.Map<turnosOperativosArea>(addTurnosOperativosAreaDto);

            _context.turnosOperativosArea.Add(turnoOperativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTurnosOperativos", new { id = turnoOperativo.idTurno }, turnoOperativo);
        }

        // PUT api/<turnosOperativosAreaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult>  PutTurnosOperativos(int id, UpdateTurnosOperativosAreaDto updateTurnosOperativosAreaDto)
        {
            var turnosOperativosArea = await _context.turnosOperativosArea.FindAsync(id);

            if (turnosOperativosArea == null)
            {
                return NotFound($"No se encontro el registro con el id: {id}");
            }

            _mapper.Map(updateTurnosOperativosAreaDto, turnosOperativosArea);
            _context.Entry(turnosOperativosArea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnosOperativosExists(id))
                {
                    return NotFound($"No se encontro el registro con el id: {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTurnosOperativosAreaDto);
        }

        private bool TurnosOperativosExists(int id)
        {
            return _context.turnosOperativosArea.Any(e => e.idTurno == id);
        }

    }
}
