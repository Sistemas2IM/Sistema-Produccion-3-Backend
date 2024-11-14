using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class cargoController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public cargoController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/cargo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<cargo>>> Getcargo()
        {
            return await _context.cargo.ToListAsync();
        }

        // GET: api/cargo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<cargo>> Getcargo(int id)
        {
            var cargo = await _context.cargo.FindAsync(id);

            if (cargo == null)
            {
                return NotFound();
            }

            return cargo;
        }

        // PUT: api/cargo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcargo(int id, cargo cargo)
        {
            if (id != cargo.idCargo)
            {
                return BadRequest();
            }

            _context.Entry(cargo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cargoExists(id))
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

        // POST: api/cargo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<cargo>> Postcargo(cargo cargo)
        {
            _context.cargo.Add(cargo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcargo", new { id = cargo.idCargo }, cargo);
        }

        // DELETE: api/cargo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletecargo(int id)
        {
            var cargo = await _context.cargo.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }

            _context.cargo.Remove(cargo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool cargoExists(int id)
        {
            return _context.cargo.Any(e => e.idCargo == id);
        }
    }
}
