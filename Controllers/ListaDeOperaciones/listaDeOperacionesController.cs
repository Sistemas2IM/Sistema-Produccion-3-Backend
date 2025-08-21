using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.ListaDeOperaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class listaDeOperacionesController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public listaDeOperacionesController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<listaDeOperacionesController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<listaDeOperacionesDto>>> GetListaDeOperaciones()
        {
            var listaDeOperaciones = await _context.listaDeOperaciones.ToListAsync();
            var listaDeOperacionesDto = _mapper.Map<List<listaDeOperacionesDto>>(listaDeOperaciones);

            return Ok(listaDeOperacionesDto);
        }

        // GET api/<listaDeOperacionesController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<listaDeOperacionesDto>> GetListaDeOperaciones(int id)
        {
            var listaDeOperaciones = await _context.listaDeOperaciones.FindAsync(id);
            var listaDeOperacionesDto = _mapper.Map<listaDeOperacionesDto>(listaDeOperaciones);

            if (listaDeOperacionesDto == null)
            {
                return NotFound($"No se encontró la operación con el id {id}");
            }

            return Ok(listaDeOperacionesDto);
        }

        // POST api/<listaDeOperacionesController>
        [HttpPost("post")]
        public async Task<ActionResult<listaDeOperaciones>> PostListaDeOperaciones(AddListaDeOperacionesDto addListaDeOperacionesDto)
        {             
            var listaDeOperaciones = _mapper.Map<listaDeOperaciones>(addListaDeOperacionesDto);
            _context.listaDeOperaciones.Add(listaDeOperaciones);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetListaDeOperaciones), new { id = listaDeOperaciones.idLista }, listaDeOperaciones);
        }

        // PUT api/<listaDeOperacionesController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutListaDeOperaciones(int id, UpdateListaDeOperacionesDto updateListaDeOperacionesDto)
        {
            var listaDeOperaciones = await _context.listaDeOperaciones.FindAsync(id);

            if (listaDeOperaciones == null)
            {
                return NotFound($"No se encontró la operación con el id {id}");
            }

            _mapper.Map(updateListaDeOperacionesDto, listaDeOperaciones);
            _context.Entry(listaDeOperaciones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaDeOperacionesExists(id))
                {
                    return NotFound($"No se encontró la operación con el id {id}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool ListaDeOperacionesExists(int id)
        {
            return _context.listaDeOperaciones.Any(e => e.idLista == id);
        }
    }
}
