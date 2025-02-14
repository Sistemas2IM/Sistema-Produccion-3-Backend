using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Etiquetas.Etiqueta
{
    [Route("api/[controller]")]
    [ApiController]
    public class etiquetaController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public etiquetaController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/etiqueta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<etiqueta>>> Getetiqueta()
        {
            return await _context.etiqueta.ToListAsync();
        }

        // GET: api/etiqueta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<etiqueta>> Getetiqueta(int id)
        {
            var etiqueta = await _context.etiqueta.FindAsync(id);

            if (etiqueta == null)
            {
                return NotFound();
            }

            return etiqueta;
        }

        // PUT: api/etiqueta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putetiqueta(int id, etiqueta etiqueta)
        {
            if (id != etiqueta.idEtiqueta)
            {
                return BadRequest();
            }

            _context.Entry(etiqueta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!etiquetaExists(id))
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

        // POST: api/etiqueta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<etiqueta>> Postetiqueta(etiqueta etiqueta)
        {
            _context.etiqueta.Add(etiqueta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getetiqueta", new { id = etiqueta.idEtiqueta }, etiqueta);
        }

        // DELETE: api/etiqueta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteetiqueta(int id)
        {
            var etiqueta = await _context.etiqueta.FindAsync(id);
            if (etiqueta == null)
            {
                return NotFound();
            }

            _context.etiqueta.Remove(etiqueta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool etiquetaExists(int id)
        {
            return _context.etiqueta.Any(e => e.idEtiqueta == id);
        }
    }
}
