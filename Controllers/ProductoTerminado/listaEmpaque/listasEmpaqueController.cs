using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.ListaEmpaque;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ProductoTerminado.listaEmpaque
{
    [Route("api/[controller]")]
    [ApiController]
    public class listasEmpaqueController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public listasEmpaqueController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<listasEmpaqueController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ListasEmpaqueDto>>> GetlistasEmpaque()
        {
            var listasEmpaque = await _context.listasEmpaque.ToListAsync();
            var listasEmpaqueDto = _mapper.Map<List<ListasEmpaqueDto>>(listasEmpaque);

            return Ok(listasEmpaqueDto);
        }


        // GET api/<listasEmpaqueController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ListasEmpaqueDto>> GetlistasEmpaque(string id)
        {
            var listasEmpaque = await _context.listasEmpaque.FindAsync(id);
            var listasEmpaqueDto = _mapper.Map<ListasEmpaqueDto>(listasEmpaque);
            if (listasEmpaqueDto == null)
            {
                return NotFound($"No se encontro la lista de empaque con el ID: {id}");
            }
            return Ok(listasEmpaqueDto);
        }

        // POST api/<listasEmpaqueController>
        [HttpPost("post")]
        public async Task<ActionResult<ListasEmpaqueDto>> PostlistasEmpaque(AddListasEmpaqueDto listasEmpaqueDto)
        {
            var listasEmpaque = _mapper.Map<listasEmpaque>(listasEmpaqueDto);
            _context.listasEmpaque.Add(listasEmpaque);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetlistasEmpaque", new { id = listasEmpaque.idEmpaque }, listasEmpaque);
        }


        // PUT api/<listasEmpaqueController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutlistasEmpaque(string id, UpdateListasEmpaqueDto updateListasEmpaque)
        {
            var listasEmpaque = await _context.listasEmpaque.FindAsync(id);
            if (listasEmpaque == null)
            {
                return NotFound($"No se encontro la lista de empaque con el ID: {id}");
            }
            _mapper.Map(updateListasEmpaque, listasEmpaque);
            _context.Entry(listasEmpaque).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!listasEmpaqueExists(id))
                {
                    return NotFound($"No se encontro la lista de empaque con el ID: {id}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool listasEmpaqueExists(string id)
        {
            return _context.listasEmpaque.Any(e => e.idEmpaque == id);
        }

    }
}
