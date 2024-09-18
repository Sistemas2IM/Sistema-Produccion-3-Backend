using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class tarjetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public tarjetaOfController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/tarjetaOf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tarjetaOf>>> GettarjetaOf(string apiKey)
        {
            return await _context.tarjetaOf.ToListAsync();
        }

        // GET: api/tarjetaOf/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tarjetaOf>> GettarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);

            if (tarjetaOf == null)
            {
                return NotFound();
            }

            return tarjetaOf;
        }

        // PUT: api/tarjetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttarjetaOf(int id, tarjetaOf tarjetaOf)
        {
            if (id != tarjetaOf.idTarjetaOf)
            {
                return BadRequest();
            }

            _context.Entry(tarjetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tarjetaOfExists(id))
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

        // POST: api/tarjetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<tarjetaOf>> PosttarjetaOf(tarjetaOf tarjetaOf)
        {
            _context.tarjetaOf.Add(tarjetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettarjetaOf", new { id = tarjetaOf.idTarjetaOf }, tarjetaOf);
        }

        // DELETE: api/tarjetaOf/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);
            if (tarjetaOf == null)
            {
                return NotFound();
            }

            _context.tarjetaOf.Remove(tarjetaOf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool tarjetaOfExists(int id)
        {
            return _context.tarjetaOf.Any(e => e.idTarjetaOf == id);
        }
    }
}
