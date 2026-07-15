using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class logProgramacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public logProgramacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<logProgramacionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<LogProgramacionDto>>> GetLogProgramacionDto()
        {
            var logProgramacion = await _context.logProgramacion
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(lpd => lpd.idProcesoNavigation)
                .ThenInclude(ip => ip.oFNavigation)    
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(es => es.estadoAnteriorNavigation)
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(es => es.estadoNuevoNavigation)
                .Include(dt => dt.tableroNavigation)
                .Include(pr => pr.programadoPorNavigation)
                .ToListAsync();

            var logProgramacionDto = _mapper.Map<IEnumerable<LogProgramacionDto>>(logProgramacion);

            return Ok(logProgramacionDto);
        }

        // GET api/<logProgramacionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<LogProgramacionDto>> GetLogProgramacionDto(int id)
        {
            var logProgramacion = await _context.logProgramacion
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(lpd => lpd.idProcesoNavigation)
                .ThenInclude(ip => ip.oFNavigation)
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(es => es.estadoAnteriorNavigation)
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(es => es.estadoNuevoNavigation)
                .Include(dt => dt.tableroNavigation)
                .Include(pr => pr.programadoPorNavigation)
                .FirstOrDefaultAsync(lp => lp.idLogProgramacion == id);

            if (logProgramacion == null)
            {
                return NotFound();
            }

            var logProgramacionDto = _mapper.Map<LogProgramacionDto>(logProgramacion);
            return Ok(logProgramacionDto);
        }

        // GET por idTablero
        [HttpGet("get/tablero/{idTablero}")]
        public async Task<ActionResult<IEnumerable<LogProgramacionDto>>> GetLogProgramacionDtoByTablero(int idTablero)
        {
            var logProgramacion = await _context.logProgramacion
                .Include(dt => dt.logProgramacionDetalle)
                .ThenInclude(lpd => lpd.idProcesoNavigation)
                .ThenInclude(ip => ip.oFNavigation)
                .Include(dt => dt.tableroNavigation)
                .Include(pr => pr.programadoPorNavigation)
                .Where(lp => lp.tablero == idTablero)
                .ToListAsync();

            var logProgramacionDto = _mapper.Map<IEnumerable<LogProgramacionDto>>(logProgramacion);

            return Ok(logProgramacionDto);
        }

        // POST api/<logProgramacionController>
        [HttpPost("post")]
        public async Task<ActionResult<logProgramacion>> PostLogProgramacion(AddLogProgramacionDto logProgramacionDto)
        {
            var logProgramacion = _mapper.Map<logProgramacion>(logProgramacionDto);
            _context.logProgramacion.Add(logProgramacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogProgramacionDto), new { id = logProgramacion.idLogProgramacion }, logProgramacion);
        }

        // PUT api/<logProgramacionController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutLogProgramacion(int id, UpdateLogProgramacionDto logProgramacionDto)
        {
            var logProgramacion = await _context.logProgramacion.FindAsync(id);
            if (logProgramacion == null)
            {
                return NotFound();
            }

            _mapper.Map(logProgramacionDto, logProgramacion);
            _context.Entry(logProgramacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogProgramacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(logProgramacion);
        }

        private bool LogProgramacionExists(int id)
        {
            return _context.logProgramacion.Any(e => e.idLogProgramacion == id);
        }

    }
}
