using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPreprensa;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaPreprensaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public infoMaquinaPreprensaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaPreprensaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaPreprensaDto>>> GetinfoMaquinaPreprensa()
        {
            var infomaquinapreprensa = await _context.infoMaquinaPreprensa.ToListAsync();
            var infomaquinapreprensaDto = _mapper.Map<List<InfoMaquinaPreprensaDto>>(infomaquinapreprensa);

            return Ok(infomaquinapreprensaDto);
        }

        // GET api/<infoMaquinaPreprensaController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaPreprensaDto>> GetinfoMaquinaPreprensa(int idmaquina)
        {
            var infomaquinapreprensa = await _context.infoMaquinaPreprensa.FindAsync(idmaquina);
            var infomaquinapreprensaDto = _mapper.Map<InfoMaquinaPreprensaDto>(infomaquinapreprensa);

            if (infomaquinapreprensaDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinapreprensaDto);
        }

        // POST api/<infoMaquinaPreprensaController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaPreprensaDto>> PostinfoMaquinaPreprensa(InfoMaquinaPreprensaDto infomaquinapreprensaDto)
        {
            var infomaquinapreprensa = _mapper.Map<infoMaquinaPreprensa>(infomaquinapreprensaDto);
            _context.infoMaquinaPreprensa.Add(infomaquinapreprensa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaPreprensa", new { idmaquina = infomaquinapreprensa.idMaquina }, infomaquinapreprensaDto);
        }

        // PUT api/<infoMaquinaPreprensaController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaPreprensa(int? idmaquina, InfoMaquinaPreprensaDto infomaquinapreprensaDto)
        {
            var infoMaquina = await _context.infoMaquinaPreprensa.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinapreprensaDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaPreprensaExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinapreprensaDto);
        }

        private bool infoMaquinaPreprensaExists(int? idmaquina)
        {
            return _context.infoMaquinaPreprensa.Any(e => e.idMaquina == idmaquina);
        }
    }
}
