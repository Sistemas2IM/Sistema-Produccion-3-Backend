using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class lotePliegoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public lotePliegoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<lotePliegoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<lotePliegoDto>>> GetSolicitud()
        {
            var lotePliego = await _context.lotePliego.ToListAsync();
            var lotePliegoDto = _mapper.Map<List<lotePliegoDto>>(lotePliego);

            return Ok(lotePliegoDto);
        }

        // GET api/<lotePliegoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<lotePliegoDto>>> GetSolicitudId(int id)
        {
            var lotePliego = await _context.lotePliego.FindAsync(id);
            var lotePliegoDto = _mapper.Map<lotePliegoDto>(lotePliego);

            if (lotePliegoDto == null)
            {
                return NotFound($"No se encontro el registro con el id: {id}");
            }

            return Ok(lotePliegoDto);
        }

        [HttpGet("get/procesoOrigen/{id}")]
        public async Task<ActionResult<IEnumerable<lotePliegoDto>>> GetLoteProcesoOrigen(int id)
        {
            var lotePliego = await _context.lotePliego
                .Where (lp => lp.procesoOrigen == id)
                .ToListAsync();

            var lotePliegoDto = _mapper.Map<List<lotePliegoDto>>(lotePliego);

            return Ok(lotePliegoDto);
        }

        [HttpGet("get/idSolicitud/{id}")]
        public async Task<ActionResult<IEnumerable<lotePliegoDto>>> GetSolicitud(int id)
        {
            var lotePliego = await _context.lotePliego
                .Where(lp => lp.idSolicitud == id)
                .ToListAsync();

            var lotePliegoDto = _mapper.Map<List<lotePliegoDto>>(lotePliego);

            return Ok(lotePliegoDto);
        }

        // POST api/<lotePliegoController>
        [HttpPost("post")]
        public async Task<ActionResult<lotePliego>> PostSolicitud(AddLotePliegoDto addLotePliegoDto)
        {
            var lotePliego = _mapper.Map<lotePliego>(addLotePliegoDto);
            _context.lotePliego.Add(lotePliego);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = lotePliego.idSolicitud }, lotePliego);
        }

        // PUT api/<lotePliegoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, UpdateLotePliegoDto updateLotePliegoDto)
        {
            var lotePliego = await _context.lotePliego.FindAsync(id);
            if (lotePliego == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(updateLotePliegoDto, lotePliego);
            _context.Entry(lotePliego).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!lotePliegoExist(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateLotePliegoDto);
        }

        private bool lotePliegoExist(int id)
        {
            return _context.lotePliego.Any(e => e.idLote == id);
        }

    }
}
