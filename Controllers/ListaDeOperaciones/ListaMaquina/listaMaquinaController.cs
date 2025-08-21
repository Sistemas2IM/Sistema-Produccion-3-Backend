using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ListaDeOperaciones.ListaMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class listaMaquinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public listaMaquinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<listaMaquinaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<listaMaquinaDto>>> GetListaMaquina()
        {
            var listaMaquinas = await _context.listaMaquina.ToListAsync();
            var listaMaquinaDto = _mapper.Map<List<listaMaquinaDto>>(listaMaquinas);

            return Ok(listaMaquinaDto);
        }

        // GET api/<listaMaquinaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<listaMaquinaDto>> GetListaMaquina(int id)
        {
            var listMaquina = await _context.listaMaquina.FindAsync(id);
            var listaMaquinaDto = _mapper.Map<listaMaquinaDto>(listMaquina);
            if (listaMaquinaDto == null)
            {
                return NotFound($"No se encontró la máquina con el id {id}");
            }
            return Ok(listaMaquinaDto);
        }

        // POST api/<listaMaquinaController>
        [HttpPost("post")]
        public async Task<ActionResult<listaItem>> PostListaMaquina(AddListaMaquinaDto addListaMaquinaDto)
        {
            var listaMaquina = _mapper.Map<listaMaquina>(addListaMaquinaDto);
            _context.listaMaquina.Add(listaMaquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetListaMaquina), new { id = listaMaquina.idListaMaquina }, listaMaquina);
        }

        // PUT api/<listaMaquinaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutListaMaquina(int id, UpdateListaMaquinaDto updateListaMaquinaDto)
        {
            var listaMaquina = await _context.listaMaquina.FindAsync(id);
            if (listaMaquina == null)
            {
                return NotFound($"No se encontró la máquina con el id {id}");
            }

            _mapper.Map(updateListaMaquinaDto, listaMaquina);
            _context.Entry(listaMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaMaquinaExists(id))
                {
                    return NotFound($"No se encontró la máquina con el id {id}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST BATCH
        [HttpPost("post/batch")]
        public async Task<IActionResult> BatchAddListaMaquina([FromBody] AddBatchListaMaquinaDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchlistaMaquinas == null || !batchAddDto.addBatchlistaMaquinas.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }
            var listaMaquinas = _mapper.Map<List<listaMaquina>>(batchAddDto.addBatchlistaMaquinas);
            _context.listaMaquina.AddRange(listaMaquinas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al agregar las máquinas.");
            }
            return Ok(new
            {
                Message = "Máquinas agregadas correctamente.",
                maquinas = listaMaquinas
            });
        }

        // DELETE BATCH
        [HttpDelete("delete/batch")]
        public async Task<IActionResult> BatchDeleteListaMaquina([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No se enviaron IDs para eliminar.");
            }

            var listaMaquinas = await _context.listaMaquina
                .Where(item => ids.Contains(item.idListaMaquina))
                .ToListAsync();

            if (!listaMaquinas.Any())
            {
                return NotFound("No se encontraron máquinas con los IDs proporcionados.");
            }

            _context.listaMaquina.RemoveRange(listaMaquinas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar las máquinas.");
            }

            return Ok("Máquinas eliminadas correctamente.");
        }

        private bool ListaMaquinaExists(int id)
        {
            return _context.listaMaquina.Any(e => e.idListaMaquina == id);
        }
    }
}
