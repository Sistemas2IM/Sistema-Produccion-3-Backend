using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.PermisoMaquina;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class permisoMaquinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public permisoMaquinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/permisoMaquina
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PermisoMaquinaDto>>> GetpermisoMaquina()
        {
            var permisoMaquina = await _context.permisoMaquina
                .ToListAsync();

            var permisoMaquinaDto = _mapper.Map<List<PermisoMaquinaDto>>(permisoMaquina);

            return Ok(permisoMaquinaDto);
        }

        // GET: api/permisoMaquinaUser
        [HttpGet("get/User/{user}")]
        public async Task<ActionResult<IEnumerable<PermisoMaquinaDto>>> GetpermisoMaquinaUser(string user)
        {
            var permisoMaquina = await _context.permisoMaquina
                .Where(u => u.user == user)
                .ToListAsync();

            var permisoMaquinaDto = _mapper.Map<List<PermisoMaquinaDto>>(permisoMaquina);

            return Ok(permisoMaquinaDto);
        }

        // GET: api/permisoMaquina/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PermisoMaquinaDto>> GetpermisoMaquina(int id)
        {
            var permisoMaquina = await _context.permisoMaquina               
                .FindAsync(id);

            var permisoMaquinaDto = _mapper.Map<PermisoMaquinaDto>(permisoMaquina);

            if (permisoMaquina == null)
            {
                return NotFound("No se encontro nigun registro de Permiso de maquina con el ID: " + id);
            }

            return Ok(permisoMaquinaDto);
        }

        // PUT: api/permisoMaquina/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutpermisoMaquina(int id, UpdatePermisoMaquinaDto updatePermisoMaquina)
        {
            var permisoMaquina = await _context.permisoMaquina.FindAsync(id);

            if (permisoMaquina == null)
            {
                return NotFound("No se encontro nigun registro de Permiso con el ID: " + id);
            }

            _mapper.Map(updatePermisoMaquina, permisoMaquina);
            _context.Entry(permisoMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!permisoMaquinaExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con ningun registro XD");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updatePermisoMaquina);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdatePermisosMaquina([FromBody] BatchUpdatePermisoMaquina batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updatePermisoMaquinas == null || !batchUpdateDto.updatePermisoMaquinas.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updatePermisoMaquinas.Select(t => t.idPermisoMaquina).ToList();

            // Obtener todas los permisos relacionadas
            var permisos = await _context.permisoMaquina.Where(t => ids.Contains(t.idPermisoMaquina)).ToListAsync();

            if (!permisos.Any())
            {
                return NotFound("No se encontraron permisos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updatePermisoMaquinas)
            {
                var permiso = permisos.FirstOrDefault(t => t.idPermisoMaquina == dto.idPermisoMaquina);
                if (permiso != null)
                {
                    permiso.user = dto.user ?? permiso.user;

                    permiso.maquina = dto.maquina ?? permiso.maquina;

                    permiso.asignada = dto.asignada ?? permiso.asignada;

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


        // POST: api/permisoMaquina
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<permisoMaquina>> PostpermisoMaquina(AddPermisoMaquinaDto addPermisoMaquina)
        {
            var permisoMaquina = _mapper.Map<permisoMaquina>(addPermisoMaquina);
            _context.permisoMaquina.Add(permisoMaquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetpermisoMaquina", new { id = permisoMaquina.idPermisoMaquina }, permisoMaquina);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddPermisoOf([FromBody] BatchAddPermisoMaquina batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addPermisoMaquinas == null || !batchAddDto.addPermisoMaquinas.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var permisosMaquinas = batchAddDto.addPermisoMaquinas.Select(dto => _mapper.Map<permisoMaquina>(dto)).ToList();

            // Agregar los permisos a la base de datos
            await _context.permisoMaquina.AddRangeAsync(permisosMaquinas);

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
                PermisosAgregados = permisosMaquinas
            });
        }

        private bool permisoMaquinaExists(int id)
        {
            return _context.permisoMaquina.Any(e => e.idPermisoMaquina == id);
        }
    }
}
