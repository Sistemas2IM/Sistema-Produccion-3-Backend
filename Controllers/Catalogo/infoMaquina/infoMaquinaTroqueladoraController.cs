using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaTroqueladora;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaTroqueladoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        public infoMaquinaTroqueladoraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaTroqueladoraController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaTroqueladoraDto>>> GetinfoMaquinaTroqueladora()
        {
            var infomaquinatroqueladora = await _context.infoMaquinaTroqueladora.ToListAsync();
            var infomaquinatroqueladoraDto = _mapper.Map<List<InfoMaquinaTroqueladoraDto>>(infomaquinatroqueladora);

            return Ok(infomaquinatroqueladoraDto);
        }

        // GET api/<infoMaquinaTroqueladoraController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaTroqueladoraDto>> GetinfoMaquinaTroqueladora(int idmaquina)
        {
            var infomaquinatroqueladora = await _context.infoMaquinaTroqueladora.FindAsync(idmaquina);
            var infomaquinatroqueladoraDto = _mapper.Map<InfoMaquinaTroqueladoraDto>(infomaquinatroqueladora);

            if (infomaquinatroqueladoraDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinatroqueladoraDto);
        }

        // POST api/<infoMaquinaTroqueladoraController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaTroqueladoraDto>> PostinfoMaquinaTroqueladora(InfoMaquinaTroqueladoraDto infomaquinatroqueladoraDto)
        {
            var infomaquinatroqueladora = _mapper.Map<infoMaquinaTroqueladora>(infomaquinatroqueladoraDto);
            _context.infoMaquinaTroqueladora.Add(infomaquinatroqueladora);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaTroqueladora", new { idmaquina = infomaquinatroqueladora.idMaquina }, infomaquinatroqueladoraDto);
        }

        // PUT api/<infoMaquinaTroqueladoraController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaTroqueladora(int? idmaquina, InfoMaquinaTroqueladoraDto infomaquinatroqueladoraDto)
        {
            var infoMaquina = await _context.infoMaquinaTroqueladora.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinatroqueladoraDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaTroqueladoraExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinatroqueladoraDto);
        }

        private bool infoMaquinaTroqueladoraExists(int? idmaquina)
        {
            return _context.infoMaquinaTroqueladora.Any(e => e.idMaquina == idmaquina);
        }
    }
}
