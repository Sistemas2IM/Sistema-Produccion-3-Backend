using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico.BatchPermisoEspecifico;

namespace Sistema_Produccion_3_Backend.Controllers.Permisos
{
    [Route("api/[controller]")]
    [ApiController]
    public class permisoEspecificoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public permisoEspecificoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PermisoEspecificoDto>>> GetPermisoEspecifico()
        {
            var permisoEspecifico = await _context.permisoEspecifico
                .Include(u => u.idRolNavigation)
                .Include(u => u.idPermisoTipoNavigation)
                .ToListAsync();

            var permisoEspecificoDto = _mapper.Map<List<PermisoEspecificoDto>>(permisoEspecifico);

            return Ok(permisoEspecificoDto);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PermisoEspecificoDto>> GetPermisoEspecifico(int id)
        {
            var permisoEspecifico = await _context.permisoEspecifico
                .Include(u => u.idRolNavigation)
                .Include(u => u.idPermisoTipoNavigation)
                .FirstOrDefaultAsync(u => u.idPermisoEspecifico == id);
            if (permisoEspecifico == null)
            {
                return NotFound();
            }
            var permisoEspecificoDto = _mapper.Map<PermisoEspecificoDto>(permisoEspecifico);
            return Ok(permisoEspecificoDto);
        }

        [HttpPost("post")]
        public async Task<ActionResult<PermisoEspecificoDto>> PostPermisoEspecifico(AddPermisoEspecificoDto permisoEspecificoDto)
        {
            var permisoEspecifico = _mapper.Map<permisoEspecifico>(permisoEspecificoDto);
            _context.permisoEspecifico.Add(permisoEspecifico);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPermisoEspecifico), new { id = permisoEspecifico.idPermisoEspecifico }, permisoEspecifico);
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutPermisoEspecifico(int id, UpdatePermisoEspecificoDto permisoEspecificoDto)
        {
            var permisoEspecifico = await _context.permisoEspecifico.FindAsync(id);

            if (permisoEspecifico == null)
            {
                return NotFound();
            }
            _mapper.Map(permisoEspecificoDto, permisoEspecifico);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoEspecificoExists(id))
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

        [HttpPut("put/batch")]
        public async Task<IActionResult> BatchUpdatePermisoEspecifico([FromBody] BatchUpdatePermisoEspecifico batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.permisosEspecificos == null || !batchUpdateDto.permisosEspecificos.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var permisos = batchUpdateDto.permisosEspecificos.Select(dto => _mapper.Map<permisoEspecifico>(dto)).ToList();

            foreach (var dto in batchUpdateDto.permisosEspecificos)
            {
                var permiso = permisos.FirstOrDefault(t => t.idPermisoEspecifico == dto.idPermisoEspecifico);
                if (permiso != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idPermisoEspecifico != 0)
                    {
                        permiso.idPermisoTipo = dto.idPermisoTipo;
                        permiso.idRol = dto.idRol;
                        permiso.habilitado = dto.habilitado;
                    }

                    _context.Entry(permiso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al actualizar los permisos específicos.");
            }
            return Ok(new { Message = "Permisos específicos actualizados exitosamente." });
        }

        [HttpPost("post/batch")]
        public async Task<IActionResult> BatchAddPermisoEspecifico([FromBody] BatchAddPermisoEspecifico batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.permisosEspecificos == null || !batchAddDto.permisosEspecificos.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var permiso = batchAddDto.permisosEspecificos.Select(dto => _mapper.Map<permisoEspecifico>(dto)).ToList();

            await _context.permisoEspecifico.AddRangeAsync(permiso);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "agregados exitosamente.",
                permisos = permiso
            });
        }

        private bool PermisoEspecificoExists(int id)
        {
            return _context.permisoEspecifico.Any(e => e.idPermisoEspecifico == id);
        }

    }
}
