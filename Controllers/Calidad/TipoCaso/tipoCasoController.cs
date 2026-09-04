using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.TipoCaso;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.TipoCaso
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoCasoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoCasoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<tipoCasoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TipoCasoDto>>> GetTipoCaso()
        {
            var tipoCaso = await _context.tipoCaso
                .ToListAsync();

            var tipoCasoDto = _mapper.Map<List<TipoCasoDto>>(tipoCaso);

            return Ok(tipoCasoDto);
        }

        // GET api/<tipoCasoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TipoCasoDto>> GetTipoCaso(int id)
        {
            var tipoCaso = await _context.tipoCaso.FindAsync(id);

            if (tipoCaso == null)
            {
                return NotFound();
            }

            var tipoCasoDto = _mapper.Map<TipoCasoDto>(tipoCaso);

            return Ok(tipoCasoDto);
        }

        // POST api/<tipoCasoController>
        [HttpPost("post")]
        public async Task<ActionResult<tipoCaso>> PostTipoCaso(AddTipoCasoDto addTipoCasoDto)
        {
            var tipoCaso = _mapper.Map<tipoCaso>(addTipoCasoDto);

            _context.tipoCaso.Add(tipoCaso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoCaso), new { id = tipoCaso.idTipoCaso }, tipoCaso);
        }

        // PUT api/<tipoCasoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutTipoCaso(int id, UpdateTipoCasoDto updateTipoCasoDto)
        {
            var tipoCaso = await _context.tipoCaso.FindAsync(id);

            if (tipoCaso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateTipoCasoDto, tipoCaso);
            _context.Entry(tipoCaso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCasoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTipoCasoDto);
        }

        private bool TipoCasoExists(int id)
        {
            return (_context.tipoCaso?.Any(e => e.idTipoCaso == id)).GetValueOrDefault();
        }
    }
}
