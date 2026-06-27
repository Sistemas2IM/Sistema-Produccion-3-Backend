using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPrensaOffset;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo.infoMaquina
{
    [Route("api/[controller]")]
    [ApiController]
    public class infoMaquinaPrensaOffsetController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public infoMaquinaPrensaOffsetController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<infoMaquinaPrensaOffsetController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<InfoMaquinaPrensaOffsetDto>>> GetinfoMaquinaPrensaOffset()
        {
            var infomaquinaprensaoffset = await _context.infoMaquinaPrensaOffset.ToListAsync();
            var infomaquinaprensaoffsetDto = _mapper.Map<List<InfoMaquinaPrensaOffsetDto>>(infomaquinaprensaoffset);

            return Ok(infomaquinaprensaoffsetDto);
        }

        // GET api/<infoMaquinaPrensaOffsetController>/5
        [HttpGet("get/{idmaquina}")]
        public async Task<ActionResult<InfoMaquinaPrensaOffsetDto>> GetinfoMaquinaPrensaOffset(int idmaquina)
        {
            var infomaquinaprensaoffset = await _context.infoMaquinaPrensaOffset.FindAsync(idmaquina);
            var infomaquinaprensaoffsetDto = _mapper.Map<InfoMaquinaPrensaOffsetDto>(infomaquinaprensaoffset);

            if (infomaquinaprensaoffsetDto == null)
            {
                return NotFound();
            }

            return Ok(infomaquinaprensaoffsetDto);
        }

        // POST api/<infoMaquinaPrensaOffsetController>
        [HttpPost("post")]
        public async Task<ActionResult<InfoMaquinaPrensaOffsetDto>> PostinfoMaquinaPrensaOffset(InfoMaquinaPrensaOffsetDto infomaquinaprensaoffsetDto)
        {
            var infomaquinaprensaoffset = _mapper.Map<infoMaquinaPrensaOffset>(infomaquinaprensaoffsetDto);
            _context.infoMaquinaPrensaOffset.Add(infomaquinaprensaoffset);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetinfoMaquinaPrensaOffset", new { idmaquina = infomaquinaprensaoffset.idMaquina }, infomaquinaprensaoffsetDto);
        }

        // PUT api/<infoMaquinaPrensaOffsetController>/5
        [HttpPut("put/{idmaquina}")]
        public async Task<IActionResult> PutinfoMaquinaPrensaOffset(int? idmaquina, InfoMaquinaPrensaOffsetDto infomaquinaprensaoffsetDto)
        {
            var infoMaquina = await _context.infoMaquinaPrensaOffset.FindAsync(idmaquina);

            if (infoMaquina == null)
            {
                return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
            }

            _mapper.Map(infomaquinaprensaoffsetDto, infoMaquina);
            _context.Entry(infoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!infoMaquinaPrensaOffsetExists(idmaquina))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(infomaquinaprensaoffsetDto);
        }

        private bool infoMaquinaPrensaOffsetExists(int? idmaquina)
        {
            return _context.infoMaquinaPrensaOffset.Any(e => e.idMaquina == idmaquina);
        }
    }
}
