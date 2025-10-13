using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaCliente
{
    [Route("api/[controller]")]
    [ApiController]
    public class fichaTecnicaClienteController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public fichaTecnicaClienteController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<fichaTecnicaClienteController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaClienteDto>>> GetFichaCliente()
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente
                .ToListAsync();

            var fichaTecnicaClienteDto = _mapper.Map<List<FichaTecnicaClienteDto>>(fichaTecnicaCliente);

            return Ok(fichaTecnicaClienteDto);
        }

        // GET api/<fichaTecnicaClienteController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FichaTecnicaClienteDto>> GetFichaCliente(int id)
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente
                .FirstOrDefaultAsync(u => u.idFichaCliente == id);

            if (fichaTecnicaCliente == null)
            {
                return NotFound();
            }

            var fichaTecnicaClienteDto = _mapper.Map<FichaTecnicaClienteDto>(fichaTecnicaCliente);
            return Ok(fichaTecnicaClienteDto);
        }

        // POST api/<fichaTecnicaClienteController>
        [HttpPost("post")]
        public async Task<ActionResult<fichaTecnicaCliente>> PostFichaCliente(AddFichaTecnicaClienteDto fichaTecnicaClienteDto)
        {
            var fichaTecnicaCliente = _mapper.Map<fichaTecnicaCliente>(fichaTecnicaClienteDto);

            _context.fichaTecnicaCliente.Add(fichaTecnicaCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFichaCliente", new { id = fichaTecnicaCliente.idFichaCliente }, fichaTecnicaCliente);
        }

        // PUT api/<fichaTecnicaClienteController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFichaCliente(int id, UpdateFichaTecnicaClienteDto updateFichaTecnicaClienteDto)
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente.FindAsync(id);

            if (fichaTecnicaCliente == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFichaTecnicaClienteDto, fichaTecnicaCliente);
            _context.Entry(fichaTecnicaCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fichaTecnicaClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFichaTecnicaClienteDto);
        }

        private bool fichaTecnicaClienteExists(int id)
        {
            return _context.fichaTecnicaCliente.Any(e => e.idFichaCliente == id);
        }
    }
}
