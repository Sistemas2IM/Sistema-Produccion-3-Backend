using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class permisoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public permisoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/permiso
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PermisoDto>>> Getpermiso()
        {
            var permiso = await _context.permiso
                .Include(u => u.idSubModuloNavigation)
                .ThenInclude(s => s.idModuloNavigation)
                .ThenInclude(m => m.idMenuNavigation)
                .ToListAsync();
            var permisoDto = _mapper.Map<List<PermisoDto>>(permiso);

            return Ok(permisoDto);
        }

        // GET: api/permiso/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PermisoDto>> Getpermiso(int id)
        {
            var permiso = await _context.permiso
                .Include(u => u.idSubModuloNavigation)
                .ThenInclude(s => s.idModuloNavigation)
                .ThenInclude(m => m.idMenuNavigation)
                .FirstOrDefaultAsync(u => u.idPermiso == id);
            var permisoDto = _mapper.Map<PermisoDto>(permiso);

            if (permisoDto == null)
            {
                return NotFound("No se encontro nigun registro de Permiso con el ID: " + id);
            }

            return Ok(permisoDto);
        }

        // PUT: api/permiso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putpermiso(int id, UpdatePermisoDto updatePermiso)
        {
            var permiso = await _context.permiso.FindAsync(id);

            if (permiso == null)
            {
                return NotFound("No se encontro el registro de Permiso con el ID: " + id);
            }

            _mapper.Map(updatePermiso, permiso);
            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!permisoExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updatePermiso);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdatePermisos([FromBody] BatchUpdatePermisoDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.Permisos == null || !batchUpdateDto.Permisos.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.Permisos.Select(t => t.idPermiso).ToList();

            // Obtener todas los permisos relacionadas
            var permisos = await _context.permiso.Where(t => ids.Contains(t.idPermiso)).ToListAsync();

            if (!permisos.Any())
            {
                return NotFound("No se encontraron permisos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.Permisos)
            {
                var permiso = permisos.FirstOrDefault(t => t.idPermiso == dto.idPermiso);
                if (permiso != null)
                {                 
                    _context.Entry(permiso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los permisos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        // POST: api/permiso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<permiso>> Postpermiso(AddPermisoDto addPermiso)
        {
            var permiso = _mapper.Map<permiso>(addPermiso);
            _context.permiso.Add(permiso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpermiso", new { id = permiso.idPermiso }, permiso);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddPermisoOf([FromBody] BatchAddPermisoDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addPermisos == null || !batchAddDto.addPermisos.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var permisos = batchAddDto.addPermisos.Select(dto => _mapper.Map<permiso>(dto)).ToList();

            // Agregar los permisos a la base de datos
            await _context.permiso.AddRangeAsync(permisos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los permisos: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Permisos agregados exitosamente.",
                PermisosAgregados = permisos
            });
        }

        private bool permisoExists(int id)
        {
            return _context.permiso.Any(e => e.idPermiso == id);
        }
    }
}
