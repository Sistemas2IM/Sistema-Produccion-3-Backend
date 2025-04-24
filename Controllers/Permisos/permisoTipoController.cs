using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoTipo;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Produccion_3_Backend.Controllers.Permisos
{
    [Route("api/[controller]")]
    [ApiController]
    public class permisoTipoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public permisoTipoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PermisoTipoDto>>> GetPermisoTipo()
        {
            var permisoTipo = await _context.permisoTipo.ToListAsync();

            var permisoTipoDto = _mapper.Map<List<PermisoTipoDto>>(permisoTipo);

            return Ok(permisoTipoDto);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PermisoTipoDto>> GetPermisoTipo(int id)
        {
            var permisoTipo = await _context.permisoTipo.FindAsync(id);
            if (permisoTipo == null)
            {
                return NotFound();
            }
            var permisoTipoDto = _mapper.Map<PermisoTipoDto>(permisoTipo);
            return Ok(permisoTipoDto);
        }

        [HttpPost("post")]
        public async Task<ActionResult<PermisoTipoDto>> PostPermisoTipo(AddPermisoTipoDto permisoTipoDto)
        {
            var permisoTipo = _mapper.Map<permisoTipo>(permisoTipoDto);
            _context.permisoTipo.Add(permisoTipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPermisoTipo), new { id = permisoTipo.idPermisoTipo }, permisoTipo);
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutPermisoTipo(int id, UpdatePermisoTipoDto permisoTipoDto)
        {
            var permisoTipo = await _context.permisoTipo.FindAsync(id);

            if (permisoTipo == null)
            {
                return NotFound();
            }
            _mapper.Map(permisoTipoDto, permisoTipo);
            _context.Entry(permisoTipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoTipoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(permisoTipoDto);
        }

        private bool PermisoTipoExists(int id)
        {
            return _context.permisoTipo.Any(e => e.idPermisoTipo == id);
        }

    }
}
