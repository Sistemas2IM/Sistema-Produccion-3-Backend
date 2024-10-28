using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class certificadoDeCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public certificadoDeCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/certificadoDeCalidad
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CertificadoCalidadDto>>> GetcertificadoDeCalidad()
        {
            var certificado = await _context.certificadoDeCalidad
                .Include(u => u.detalleCertificado)
                .ThenInclude(r => r.idCaracteristaNavigation)
                .ToListAsync();

            var certificadoDto = _mapper.Map<List<CertificadoCalidadDto>>(certificado);

            return Ok(certificadoDto);
        }

        // GET: api/certificadoDeCalidad/5
        [HttpGet("get/id/")]
        public async Task<ActionResult<CertificadoCalidadDto>> GetcertificadoDeCalidad(int id)
        {
            var certificadoDeCalidad = await _context.certificadoDeCalidad
                .Include(u => u.detalleCertificado)
                .ThenInclude(r => r.idCaracteristaNavigation)
                .FirstOrDefaultAsync(u => u.idCertificado == id);
            var certificadoDto = _mapper.Map<CertificadoCalidadDto>(certificadoDeCalidad);

            if (certificadoDto == null)
            {
                return NotFound("No se encontro el certificado con el id: " + id);
            }         

            return Ok(certificadoDto);
        }

        // PUT: api/certificadoDeCalidad/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/")]
        public async Task<IActionResult> PutcertificadoDeCalidad(int id, UpdateCertificadoCalidadDto updateCertificadoCalidad)
        {
            var certificado = await _context.certificadoDeCalidad.FindAsync(id);

            if (certificado == null)
            {
                return NotFound("No se encontro el certficado con el id: " + id);
            }

            _mapper.Map(updateCertificadoCalidad, certificado);
            _context.Entry(certificado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!certificadoDeCalidadExists(id))
                {
                    return NotFound("No se encontro el certificado con el id: " + id);
                }
                else
                {
                    throw;
                }            
            }

            return Ok(updateCertificadoCalidad);
        }

        // POST: api/certificadoDeCalidad
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<certificadoDeCalidad>> PostcertificadoDeCalidad(AddCertificadoCalidadDto addCertificadoCalidad)
        {
            var certificado = _mapper.Map<certificadoDeCalidad>(addCertificadoCalidad);

            _context.certificadoDeCalidad.Add(certificado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificado", new { id = certificado.idCertificado }, certificado);
        }

        // DELETE: api/certificadoDeCalidad/5
        /*[HttpDelete("{id}")]
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
        }*/

        private bool certificadoDeCalidadExists(int id)
        {
            return _context.certificadoDeCalidad.Any(e => e.idCertificado == id);
        }
    }
}
