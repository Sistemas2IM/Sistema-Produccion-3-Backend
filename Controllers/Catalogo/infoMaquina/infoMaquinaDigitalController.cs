using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaDigital;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaDigitalController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public infoMaquinaDigitalController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaDigitalController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaDigitalDto>>> GetinfoMaquinaDigital()
        {
            var infomaquinadigital = await _context.infoMaquinaDigital.ToListAsync();
            var infomaquinadigitalDto = _mapper.Map<List<InfoMaquinaDigitalDto>>(infomaquinadigital);

            return Ok(infomaquinadigitalDto);
        }

        // GET api/<infoMaquinaDigitalController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaDigitalDto>> GetinfoMaquinaDigital(int idmaquina)
        {
            var infomaquinadigital = await _context.infoMaquinaDigital.FindAsync(idmaquina);
            var infomaquinadigitalDto = _mapper.Map<InfoMaquinaDigitalDto>(infomaquinadigital);

            if (infomaquinadigitalDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinadigitalDto);
        }

        // POST api/<infoMaquinaDigitalController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaDigitalDto>> PostinfoMaquinaDigital(InfoMaquinaDigitalDto infomaquinadigitalDto)
        {
            var infomaquinadigital = _mapper.Map<infoMaquinaDigital>(infomaquinadigitalDto);
            _context.infoMaquinaDigital.Add(infomaquinadigital);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaDigital", new { idmaquina = infomaquinadigital.idMaquina }, infomaquinadigitalDto);
        }

        // PUT api/<infoMaquinaDigitalController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaDigital(int? idmaquina, InfoMaquinaDigitalDto infomaquinadigitalDto)
        {
            var infoMaquina = await _context.infoMaquinaDigital.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinadigitalDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaDigitalExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinadigitalDto);
        }

        private bool infoMaquinaDigitalExists(int? idmaquina)
        {
            return _context.infoMaquinaDigital.Any(e => e.idMaquina == idmaquina);
        }
    }
}
