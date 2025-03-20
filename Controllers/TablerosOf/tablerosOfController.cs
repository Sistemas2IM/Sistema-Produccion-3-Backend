using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class tablerosOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tablerosOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tablerosOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TablerosOfDto>>> GettablerosOf()
        {
            var tablero = await _context.tablerosOf
                .Include(u => u.posturasOf)
                .Include(m => m.idMaquinaNavigation)
                .ThenInclude(f => f.idFamiliaNavigation)
                .ToListAsync();

            var tableroDto = _mapper.Map<List<TablerosOfDto>>(tablero);

            return Ok(tableroDto);
        }

        // GET: api/tablerosOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TablerosOfDto>> GettablerosOf(int id)
        {
            var tablero = await _context.tablerosOf
                .Include(u => u.posturasOf)
                .FirstOrDefaultAsync(u => u.idTablero == id);

            var tableroDto = _mapper.Map<TablerosOfDto>(tablero);

            if (tableroDto == null)
            {
                return NotFound("No se encontro el Tablero con el ID: " + id);
            }

            return Ok(tableroDto);
        }

        // GET: api/tablerosOf/5 POR ID DE MAQUINA
        [HttpGet("get/maquina/{id}")]
        public async Task<ActionResult<TablerosOfDto>> GettablerosOfMaquina(int id)
        {
            var tablero = await _context.tablerosOf
                .Include(u => u.posturasOf)
                .FirstOrDefaultAsync(u => u.idMaquina == id);

            var tableroDto = _mapper.Map<TablerosOfDto>(tablero);

            if (tableroDto == null)
            {
                return NotFound("No se encontro el Tablero con el ID de Maquina: " + id);
            }

            return Ok(tableroDto);
        }

        // PUT: api/tablerosOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttablerosOf(int id, UpdateTablerosOfDto updateTablerosOf)
        {
            var tablero = await _context.tablerosOf.FindAsync(id);

            if (tablero == null)
            {
                return NotFound("No se encontro el Tablero con el ID: " + id);
            }

            _mapper.Map(updateTablerosOf, tablero);
            _context.Entry(tablero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tablerosOfExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el resgistro");
                }
                else
                {
                    throw;
                }
                
            }
            return Ok(updateTablerosOf);
        }

        // POST: api/tablerosOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tablerosOf>> PosttablerosOf(AddTablerosOfDto addTablerosOf)
        {
            var tablero = _mapper.Map<tablerosOf>(addTablerosOf);
            _context.tablerosOf.Add(tablero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettablerosOf", new { id = tablero.idTablero }, tablero);
        }

        private bool tablerosOfExists(int id)
        {
            return _context.tablerosOf.Any(e => e.idTablero == id);
        }
    }
}
