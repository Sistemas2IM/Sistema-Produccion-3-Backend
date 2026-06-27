using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPegadora;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaPegadoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public infoMaquinaPegadoraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/<infoMaquinaPegadoraController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaPegadoraDto>>> GetinfoMaquinaPegadora()
        {
            var infomaquinapegadora = await _context.infoMaquinaPegadora.ToListAsync();
            var infomaquinapegadoraDto = _mapper.Map<List<InfoMaquinaPegadoraDto>>(infomaquinapegadora);

            return Ok(infomaquinapegadoraDto);
        }

        // GET api/<infoMaquinaPegadoraController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaPegadoraDto>> GetinfoMaquinaPegadora(int idmaquina)
        {
            var infomaquinapegadora = await _context.infoMaquinaPegadora.FindAsync(idmaquina);
            var infomaquinapegadoraDto = _mapper.Map<InfoMaquinaPegadoraDto>(infomaquinapegadora);

            if (infomaquinapegadoraDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinapegadoraDto);
        }

        // POST api/<infoMaquinaPegadoraController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaPegadoraDto>> PostinfoMaquinaPegadora(InfoMaquinaPegadoraDto infomaquinapegadoraDto)
        {
            var infomaquinapegadora = _mapper.Map<infoMaquinaPegadora>(infomaquinapegadoraDto);
            _context.infoMaquinaPegadora.Add(infomaquinapegadora);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaPegadora", new { idmaquina = infomaquinapegadora.idMaquina }, infomaquinapegadoraDto);
        }

        // PUT api/<infoMaquinaPegadoraController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaPegadora(int? idmaquina, InfoMaquinaPegadoraDto infomaquinapegadoraDto)
        {
            var infoMaquina = await _context.infoMaquinaPegadora.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinapegadoraDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaPegadoraExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinapegadoraDto);
        }
        private bool infoMaquinaPegadoraExists(int? idmaquina)
        {
            return _context.infoMaquinaPegadora.Any(e => e.idMaquina == idmaquina);
        }
    }
}
