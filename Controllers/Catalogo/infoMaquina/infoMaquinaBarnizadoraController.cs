using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaBarnizadora;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaBarnizadoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        public infoMaquinaBarnizadoraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaBarnizadoraController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaBarnizadoraDto>>> GetinfoMaquinaBarnizadora()
        {
            var infomaquinabarnizadora = await _context.infoMaquinaBarnizadora.ToListAsync();
            var infomaquinabarnizadoraDto = _mapper.Map<List<InfoMaquinaBarnizadoraDto>>(infomaquinabarnizadora);

            return Ok(infomaquinabarnizadoraDto);
        }

        // GET api/<infoMaquinaBarnizadoraController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaBarnizadoraDto>> GetinfoMaquinaBarnizadora(int idmaquina)
        {
            var infomaquinabarnizadora = await _context.infoMaquinaBarnizadora.FindAsync(idmaquina);
            var infomaquinabarnizadoraDto = _mapper.Map<InfoMaquinaBarnizadoraDto>(infomaquinabarnizadora);

            if (infomaquinabarnizadoraDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinabarnizadoraDto);
        }

        // POST api/<infoMaquinaBarnizadoraController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaBarnizadoraDto>> PostinfoMaquinaBarnizadora(InfoMaquinaBarnizadoraDto infomaquinabarnizadoraDto)
        {
            var infomaquinabarnizadora = _mapper.Map<infoMaquinaBarnizadora>(infomaquinabarnizadoraDto);
            _context.infoMaquinaBarnizadora.Add(infomaquinabarnizadora);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaBarnizadora", new { idmaquina = infomaquinabarnizadora.idMaquina }, infomaquinabarnizadoraDto);
        }

        // PUT api/<infoMaquinaBarnizadoraController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaBarnizadora(int? idmaquina, InfoMaquinaBarnizadoraDto infomaquinabarnizadoraDto)
        {
            var infoMaquina = await _context.infoMaquinaBarnizadora.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinabarnizadoraDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaBarnizadoraExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinabarnizadoraDto);
        }

        // DELETE api/<infoMaquinaBarnizadoraController>/5
        [HttpDelete("delete/{idmaquina}")]
        public async Task<IActionResult> DeleteinfoMaquinaBarnizadora(int idmaquina)
        {
            var infoMaquina = await _context.infoMaquinaBarnizadora.FindAsync(idmaquina);
            if (infoMaquina == null)
            {
                return NotFound();
            }

            _context.infoMaquinaBarnizadora.Remove(infoMaquina);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool infoMaquinaBarnizadoraExists(int? idmaquina)
        {
            return _context.infoMaquinaBarnizadora.Any(e => e.idMaquina == idmaquina);
        }
    }
}
