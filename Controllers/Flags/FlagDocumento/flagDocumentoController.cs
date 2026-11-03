using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services.Flags;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Flags.FlagDocumento
{
    [Route("api/[controller]")]
    [ApiController]
    public class flagDocumentoController : ControllerBase
    {
        private readonly ILogger<flagDocumentoController> _logger;
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly IFlagService _flagService;

        public flagDocumentoController(ILogger<flagDocumentoController> logger, base_nuevaContext context, IMapper mapper, IFlagService flagService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _flagService = flagService;
        }

        // GET: api/<flagDocumentoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FlagDocumentoDto>>> GetFlagDocumentos()
        {
            var flagDocumentos = await _context.flagDocumento.ToListAsync();

            var flagDocumentoDtos = _mapper.Map<IEnumerable<FlagDocumentoDto>>(flagDocumentos);

            return Ok(flagDocumentoDtos);
        }

        // GET api/<flagDocumentoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FlagDocumentoDto>> GetFlagDocumento(int id)
        {
            var flagDocumento = await _context.flagDocumento
                .Include(f => f.idFlagNavigation)
                .FirstOrDefaultAsync(f => f.idFlagDocumento == id);
            if (flagDocumento == null)
            {
                return NotFound();
            }
            var flagDocumentoDto = _mapper.Map<FlagDocumentoDto>(flagDocumento);
            return Ok(flagDocumentoDto);
        }

        // POST api/<flagDocumentoController>/post
        [HttpPost("post")]
        public async Task<ActionResult<FlagDocumentoDto>> PostFlagDocumento(AddFlagDocumentoDto addFlagDocumentoDto)
        {
            try
            {
                var flagDocumentoDto = await _flagService.AsignarManualAsync(addFlagDocumentoDto);
                return CreatedAtAction(nameof(GetFlagDocumento), new { id = flagDocumentoDto.idFlagDocumento }, flagDocumentoDto);
            }
            catch (FlagValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FlagNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (FlagConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT api/<flagDocumentoController>/5
        [HttpPut("put/{id}/desactivar")]
        public async Task<IActionResult> PutFlagDocumento(int id, UpdateFlagDocumentoDto updateFlagDocumentoDto)
        {
            var flagDocumento = await _context.flagDocumento.FindAsync(id);
            if (flagDocumento == null)
            {
                return NotFound();
            }
            // Map the updated properties from the DTO to the entity
            _mapper.Map(updateFlagDocumentoDto, flagDocumento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlagDocumentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool FlagDocumentoExists(int id)
        {
            return _context.flagDocumento.Any(e => e.idFlagDocumento == id);
        }

    }
}
