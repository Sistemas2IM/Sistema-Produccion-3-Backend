using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class asignacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public asignacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/asignacion
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AsignacionDto>>> Getasignacion()
        {
            var asignacion = await _context.asignacion
                .Include(u => u.userNavigation)
                .Include(r => r.idProcesoNavigation)
                .ToListAsync();

            var asignacionDto = _mapper.Map<List<AsignacionDto>>(asignacion);

            return Ok(asignacionDto);
        }

        // GET: api/asignacion/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<AsignacionDto>> Getasignacion(int id)
        {
            var asignacion = await _context.asignacion
                .Include(u => u.userNavigation)
                .Include(r => r.idProcesoNavigation)
                .FirstOrDefaultAsync(U => U.idAsignacion == id);
            var asignacionDto = _mapper.Map<AsignacionDto>(asignacion);

            if (asignacion == null)
            {
                return NotFound("No se encontro el registro con el id: " + id);
            }

            return Ok(asignacionDto);
        }

        // PUT: api/asignacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putasignacion(int id, UpdateAsignacionDto updateAsignacion)
        {
            var asignacionOf = await _context.asignacion.FindAsync(id);

            if (asignacionOf == null)
            {
                return BadRequest("No se encontro el registro con el id: " + id);
            }

            _mapper.Map(updateAsignacion, asignacionOf);
            _context.Entry(asignacionOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!asignacionExists(id))
                {
                    return NotFound("No se encontro la Asignacion con id " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateAsignacion);
        }

        // POST: api/asignacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<asignacion>> Postasignacion(AddAsignacionDto addAsignacion)
        {
            var asignacionOf = _mapper.Map<asignacion>(addAsignacion);

            _context.asignacion.Add(asignacionOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getasignacion", new { id = asignacionOf.idAsignacion }, asignacionOf);
        }     

        // DELETE: api/asignacion/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Deleteasignacion(int id)
        {
            var asignacion = await _context.asignacion.FindAsync(id);
            if (asignacion == null)
            {
                return NotFound();
            }

            _context.asignacion.Remove(asignacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool asignacionExists(int id)
        {
            return _context.asignacion.Any(e => e.idAsignacion == id);
        }
    }
}
