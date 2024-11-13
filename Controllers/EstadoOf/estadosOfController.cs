using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.EstadoOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadosOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public estadosOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/estadosOf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoOfDto>>> GetestadosOf()
        {
            //return await _context.estadosOf.ToListAsync();
            /*
             var tarjetaOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
             */
            var estadoof = await _context.estadosOf
                .Include(t => t.tarjetaOf)
                .ToListAsync();

            var estadoOfDto = _mapper.Map<List<EstadoOfDto>>(estadoof);

            return Ok(estadoOfDto);
        }

        // GET: api/estadosOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<estadosOf>> GetestadosOf(int id)
        {
            var estadosOf = await _context.estadosOf.FindAsync(id);

            if (estadosOf == null)
            {
                return NotFound();
            }

            return estadosOf;
        }

        // PUT: api/estadosOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutestadosOf(int id, estadosOf estadosOf)
        {
            if (id != estadosOf.idEstadoOf)
            {
                return BadRequest();
            }

            _context.Entry(estadosOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!estadosOfExists(id))
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

        // POST: api/estadosOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<estadosOf>> PostestadosOf(estadosOf estadosOf)
        {
            _context.estadosOf.Add(estadosOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetestadosOf", new { id = estadosOf.idEstadoOf }, estadosOf);
        }

        // DELETE: api/estadosOf/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteestadosOf(int id)
        {
            var estadosOf = await _context.estadosOf.FindAsync(id);
            if (estadosOf == null)
            {
                return NotFound();
            }

            _context.estadosOf.Remove(estadosOf);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool estadosOfExists(int id)
        {
            return _context.estadosOf.Any(e => e.idEstadoOf == id);
        }
    }
}
