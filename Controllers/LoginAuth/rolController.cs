using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class rolController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public rolController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/rol
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<RolDto>>> Getrol()
        {
            var rol = await _context.rol.ToListAsync();
            var rolDto = _mapper.Map<List<RolDto>>(rol);

            return Ok(rolDto);
        }

        [HttpGet("get/permisos")]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetRolPermisos()
        {
            var rol = await _context.rol
                .Include(u => u.permiso)
                .ThenInclude(s => s.idSubModuloNavigation)
                .ThenInclude(m => m.idModuloNavigation)
                .ThenInclude(e => e.idMenuNavigation)
                .ToListAsync();
            var rolDto = _mapper.Map<List<RolDto>>(rol);

            return Ok(rolDto);
        }

        // GET: api/rol/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<RolDto>> Getrol(int id)
        {
            var rol = await _context.rol
                .Include(u => u.permiso)
                .ThenInclude(s => s.idSubModuloNavigation)
                .ThenInclude(m => m.idModuloNavigation)
                .ThenInclude(e => e.idMenuNavigation)
                .FirstOrDefaultAsync(u => u.idRol == id);
            var rolDto = _mapper.Map<RolDto>(rol);

            if (rolDto == null)
            {
                return NotFound("No se encontro el rol con el id: " + id);
            }

            return Ok(rolDto);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateRoles([FromBody] BatchUpdateRolDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchRol == null || !batchUpdateDto.updateBatchRol.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchRol.Select(t => t.idRol).ToList();

            // Obtener todos los roles relacionados
            var roles = await _context.rol.Where(t => ids.Contains(t.idRol)).ToListAsync();

            if (!roles.Any())
            {
                return NotFound("No se encontraron roles para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchRol)
            {
                var rol = roles.FirstOrDefault(t => t.idRol == dto.idRol);
                if (rol != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.status.HasValue)
                    {
                        rol.status = dto.status.Value;
                    }

                    _context.Entry(rol).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los roles.");
            }

            return Ok("Actualización realizada correctamente.");
        }


        // PUT: api/rol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutRol(int id, UpdateRolDto updateRol)
        {
            var rol = await _context.rol.FindAsync(id);

            if (rol ==  null)
            {
                return NotFound($"No se encontro el registro de Permiso con el ID: {id}");
            }

            _mapper.Map(updateRol, rol);
            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateRol);
        }


        // POST: api/rol
        [HttpPost("post")]
        public async Task<ActionResult<rol>> PostRol(AddRolDto addRol)
        {
            // Mapear el DTO a la entidad principal (rol)
            var rol = _mapper.Map<rol>(addRol);

            // Asegurarse de que los permisos estén vinculados al rol
            if (addRol.addPermisos != null && addRol.addPermisos.Any())
            {
                rol.permiso = addRol.addPermisos.Select(dto => _mapper.Map<permiso>(dto)).ToList();
            }

            // Agregar el rol con sus permisos a la base de datos
            _context.rol.Add(rol);
            await _context.SaveChangesAsync();

            // Retornar la respuesta
            return CreatedAtAction("GetRol", new { id = rol.idRol }, rol);
        }


        private bool rolExists(int id)
        {
            return _context.rol.Any(e => e.idRol == id);
        }
    }
}
