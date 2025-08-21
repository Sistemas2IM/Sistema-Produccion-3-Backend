using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem.Batch;
using SAPbobsCOM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ListaDeOperaciones.ListaItem
{
    [Route("api/[controller]")]
    [ApiController]
    public class listaItemController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public listaItemController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<listaItemController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<listaItemDto>>> GetListaItem()
        {             
            var listaItems = await _context.listaItem.ToListAsync();
            var listaItemDto = _mapper.Map<List<listaItemDto>>(listaItems);

            return Ok(listaItemDto);
        }

        // GET api/<listaItemController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<listaItemDto>> GetListaItem(int id)
        {
            var listaItem = await _context.listaItem.FindAsync(id);
            var listaItemDto = _mapper.Map<listaItemDto>(listaItem);
            if (listaItemDto == null)
            {
                return NotFound($"No se encontró el item con el id {id}");
            }
            return Ok(listaItemDto);
        }

        // POST api/<listaItemController>
        [HttpPost("post")]
        public async Task<ActionResult<listaItem>> PostListaItem(AddListaItemDto addListaItemDto)
        {
            var listaItem = _mapper.Map<listaItem>(addListaItemDto);
            _context.listaItem.Add(listaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetListaItem), new { id = listaItem.idListaItem }, listaItem);
        }

        // PUT api/<listaItemController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutListaItem(int id, UpdateListaItemDto updateListaItemDto)
        {
            var listaItem = await _context.listaItem.FindAsync(id);

            if (listaItem == null)
            {
                return NotFound($"No se encontró el item con el id {id}");
            }

            _mapper.Map(updateListaItemDto, listaItem);
            _context.Entry(listaItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaItemExists(id))
                {
                    return NotFound($"No se encontró el item con el id {id}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddListaItem([FromBody] AddBatchListaItemDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchListaItemDto == null || !batchAddDto.addBatchListaItemDto.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }
            var listaItems = _mapper.Map<List<listaItem>>(batchAddDto.addBatchListaItemDto);
            _context.listaItem.AddRange(listaItems);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al agregar los items.");
            }
            return Ok(new
            {
                Message = "Items agregados correctamente.",
                procesos = listaItems
            });

        }

        // DELETE BATCH
        [HttpDelete("delete/batch")]
        public async Task<IActionResult> DeleteBatchListaItem([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No se enviaron IDs para eliminar.");
            }

            var listaItems = await _context.listaItem
                .Where(item => ids.Contains(item.idListaItem))
                .ToListAsync();

            if (!listaItems.Any())
            {
                return NotFound("No se encontraron items con los IDs proporcionados.");
            }

            _context.listaItem.RemoveRange(listaItems);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(500, "Error al eliminar las operaciones :(");
            }

            return Ok($"Se eliminaron las operaciones :)");
        }

        private bool ListaItemExists(int id)
        {
            return _context.listaItem.Any(e => e.idListaItem == id);
        }
    }
}
