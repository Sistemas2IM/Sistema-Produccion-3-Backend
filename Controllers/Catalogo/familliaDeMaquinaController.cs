using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class familliaDeMaquinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public familliaDeMaquinaController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/familliaDeMaquina
        [HttpGet]
        public async Task<ActionResult<IEnumerable<familliaDeMaquina>>> GetfamilliaDeMaquina()
        {
            return await _context.familliaDeMaquina.ToListAsync();
        }

        // GET: api/familliaDeMaquina/5
        [HttpGet("{id}")]
        public async Task<ActionResult<familliaDeMaquina>> GetfamilliaDeMaquina(int id)
        {
            var familliaDeMaquina = await _context.familliaDeMaquina.FindAsync(id);

            if (familliaDeMaquina == null)
            {
                return NotFound();
            }

            return familliaDeMaquina;
        }

        // PUT: api/familliaDeMaquina/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutfamilliaDeMaquina(int id, familliaDeMaquina familliaDeMaquina)
        {
            if (id != familliaDeMaquina.idFamilia)
            {
                return BadRequest();
            }

            _context.Entry(familliaDeMaquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!familliaDeMaquinaExists(id))
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

        // POST: api/familliaDeMaquina
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<familliaDeMaquina>> PostfamilliaDeMaquina(familliaDeMaquina familliaDeMaquina)
        {
            _context.familliaDeMaquina.Add(familliaDeMaquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetfamilliaDeMaquina", new { id = familliaDeMaquina.idFamilia }, familliaDeMaquina);
        }

        // DELETE: api/familliaDeMaquina/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletefamilliaDeMaquina(int id)
        {
            var familliaDeMaquina = await _context.familliaDeMaquina.FindAsync(id);
            if (familliaDeMaquina == null)
            {
                return NotFound();
            }

            _context.familliaDeMaquina.Remove(familliaDeMaquina);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool familliaDeMaquinaExists(int id)
        {
            return _context.familliaDeMaquina.Any(e => e.idFamilia == id);
        }
    }
}
