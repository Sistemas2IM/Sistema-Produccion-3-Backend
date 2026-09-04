using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.BitacoraCaso
{
    [Route("api/[controller]")]
    [ApiController]
    public class bitacoraCasoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public bitacoraCasoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<bitacoraCasoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<BitacoraCasoDto>>> GetBitacoraCaso()
        {
            var bitacoraCaso = await _context.bitacoraCaso
                .Include(te => te.idTipoEventoNavigation)
                .Include(te => te.estadoAnteriorNavigation)
                .Include(te => te.estadoNuevoNavigation)
                .Include(te => te.idDictamenNavigation)
                .Include(te => te.idAnexoNavigation)
                //.Include(te => te.usuarioNavigation)
                .ToListAsync();

            var bitacoraCasoDto = _mapper.Map<List<BitacoraCasoDto>>(bitacoraCaso);

            return Ok(bitacoraCasoDto);
        }

        // GET api/<bitacoraCasoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<BitacoraCasoDto>> GetBitacoraCaso(int id)
        {
            var bitacoraCaso = await _context.bitacoraCaso
                .Include(te => te.idTipoEventoNavigation)
                .Include(te => te.estadoAnteriorNavigation)
                .Include(te => te.estadoNuevoNavigation)
                .Include(te => te.idDictamenNavigation)
                .Include(te => te.idAnexoNavigation)
                //.Include(te => te.usuarioNavigation)
                .FirstOrDefaultAsync(te => te.idEvento == id);

            if (bitacoraCaso == null)
            {
                return NotFound();
            }

            var bitacoraCasoDto = _mapper.Map<BitacoraCasoDto>(bitacoraCaso);

            return Ok(bitacoraCasoDto);
        }

        // POST api/<bitacoraCasoController>
        [HttpPost("post")]
        public async Task<ActionResult<BitacoraCasoResponseDTO>> PostBitacoraCaso([FromBody] AddBitacoraCasoDto addBitacoraCasoDto)
        {
            var bitacoraCaso = _mapper.Map<bitacoraCaso>(addBitacoraCasoDto);

            _context.bitacoraCaso.Add(bitacoraCaso);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetBitacoraCaso),
                new { id = bitacoraCaso.idEvento }, 
                new BitacoraCasoResponseDTO
                {
                    idEvento = bitacoraCaso.idEvento,
                    Message = "Bitácora de caso creada exitosamente."
                }
                );
        }

        // PUT api/<bitacoraCasoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutBitacoraCaso(int id, [FromBody] UpdateBitacoraCasoDto updateBitacoraCasoDto)
        {
            var bitacoraCaso = await _context.bitacoraCaso.FindAsync(id);

            if (bitacoraCaso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateBitacoraCasoDto, bitacoraCaso);
            _context.Entry(bitacoraCaso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BitacoraCasoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateBitacoraCasoDto);
        }

        private bool BitacoraCasoExists(int id)
        {
            return _context.bitacoraCaso.Any(e => e.idEvento == id);
        }
    }
}
