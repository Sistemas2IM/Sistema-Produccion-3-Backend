using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaFlexografia;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaFlexografiaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public infoMaquinaFlexografiaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaFlexografiaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaFlexografiaDto>>> GetInfoMaquinaFlexografia()
        {
            var infoMaquinaFlexografia = await _context.infoMaquinaFlexografia.ToListAsync();
            var infoMaquinaFlexografiaDto = _mapper.Map<List<InfoMaquinaFlexografiaDto>>(infoMaquinaFlexografia);

            return Ok(infoMaquinaFlexografiaDto);
        }

        // GET api/<infoMaquinaFlexografiaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<InfoMaquinaFlexografiaDto>> GetInfoMaquinaFlexografia(int id)
        {
            var infoMaquinaFlexografia = await _context.infoMaquinaFlexografia.FindAsync(id);
            var infoMaquinaFlexografiaDto = _mapper.Map<InfoMaquinaFlexografiaDto>(infoMaquinaFlexografia);

            if (infoMaquinaFlexografiaDto == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {id}");
            }

            return Ok(infoMaquinaFlexografiaDto);
        }

        // POST api/<infoMaquinaFlexografiaController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaFlexografiaDto>> PostInfoMaquinaFlexografia(InfoMaquinaFlexografiaDto addInfoMaquinaFlexografia)
        {
            var infoMaquinaFlexografia = _mapper.Map<infoMaquinaFlexografia>(addInfoMaquinaFlexografia);
            _context.infoMaquinaFlexografia.Add(infoMaquinaFlexografia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInfoMaquinaFlexografia", new { id = infoMaquinaFlexografia.idMaquina }, infoMaquinaFlexografia);
        }

        // PUT api/<infoMaquinaFlexografiaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutInfoMaquinaFlexografia(int id, InfoMaquinaFlexografiaDto updateInfoMaquinaFlexografia)
        {
            var infoMaquinaFlexografia = await _context.infoMaquinaFlexografia.FindAsync(id);

            if (infoMaquinaFlexografia == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {id}");
            }

            _mapper.Map(updateInfoMaquinaFlexografia, infoMaquinaFlexografia);
            _context.Entry(infoMaquinaFlexografia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InfoMaquinaFlexografiaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateInfoMaquinaFlexografia);
        }

        private bool InfoMaquinaFlexografiaExists(int? id)
        {
            return _context.infoMaquinaFlexografia.Any(e => e.idMaquina == id);
        }
    }
}

