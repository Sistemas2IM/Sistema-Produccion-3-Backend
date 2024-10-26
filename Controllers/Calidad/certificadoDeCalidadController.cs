using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class certificadoDeCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;

        public certificadoDeCalidadController(base_nuevaContext context)
        {
            _context = context;
        }

        // GET: api/certificadoDeCalidad
        [HttpGet]
        public async Task<ActionResult<IEnumerable<certificadoDeCalidad>>> GetcertificadoDeCalidad()
        {
            return await _context.certificadoDeCalidad.ToListAsync();
        }

        // GET: api/certificadoDeCalidad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<certificadoDeCalidad>> GetcertificadoDeCalidad(int id)
        {
            var certificadoDeCalidad = await _context.certificadoDeCalidad.FindAsync(id);

            if (certificadoDeCalidad == null)
            {
                return NotFound();
            }

            return certificadoDeCalidad;
        }

        // PUT: api/certificadoDeCalidad/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutcertificadoDeCalidad(int id, certificadoDeCalidad certificadoDeCalidad)
        {
            if (id != certificadoDeCalidad.idCertificado)
            {
                return BadRequest();
            }

            _context.Entry(certificadoDeCalidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!certificadoDeCalidadExists(id))
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

        // POST: api/certificadoDeCalidad
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<certificadoDeCalidad>> PostcertificadoDeCalidad(certificadoDeCalidad certificadoDeCalidad)
        {
            _context.certificadoDeCalidad.Add(certificadoDeCalidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetcertificadoDeCalidad", new { id = certificadoDeCalidad.idCertificado }, certificadoDeCalidad);
        }

        // DELETE: api/certificadoDeCalidad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletecertificadoDeCalidad(int id)
        {
            var certificadoDeCalidad = await _context.certificadoDeCalidad.FindAsync(id);
            if (certificadoDeCalidad == null)
            {
                return NotFound();
            }

            _context.certificadoDeCalidad.Remove(certificadoDeCalidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool certificadoDeCalidadExists(int id)
        {
            return _context.certificadoDeCalidad.Any(e => e.idCertificado == id);
        }
    }
}
