using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.EtiquetaOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class etiquetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public etiquetaOfController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/etiquetaOf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<etiquetaOf>>> GetetiquetaOf()
        {
            return await _context.etiquetaOf.ToListAsync();
        }

        // GET: api/etiquetaOf/5
        [HttpGet("{id}")]
        public async Task<ActionResult<etiquetaOf>> GetetiquetaOf(int id)
        {
            var etiquetaOf = await _context.etiquetaOf.FindAsync(id);

            if (etiquetaOf == null)
            {
                return NotFound();
            }

            return etiquetaOf;
        }

        // PUT: api/etiquetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, etiquetaOf etiquetaOf)
        {
            if (id != etiquetaOf.idEtiquetaOf)
            {
                return BadRequest();
            }

            _context.Entry(etiquetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!etiquetaOfExists(id))
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

        // POST: api/etiquetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<etiquetaOf>> PostetiquetaOf(etiquetaOf etiquetaOf)
        {
            _context.etiquetaOf.Add(etiquetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetetiquetaOf", new { id = etiquetaOf.idEtiquetaOf }, etiquetaOf);
        }

        // DELETE: api/etiquetaOf/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteetiquetaOf(int id)
        {
            var etiquetaOf = await _context.etiquetaOf.FindAsync(id);
            if (etiquetaOf == null)
            {
                return NotFound();
            }

            _context.etiquetaOf.Remove(etiquetaOf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool etiquetaOfExists(int id)
        {
            return _context.etiquetaOf.Any(e => e.idEtiquetaOf == id);
        }
    }
}
