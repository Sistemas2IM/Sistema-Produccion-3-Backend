using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleImpresionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleImpresionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleImpresion
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DetalleImpresionDto>>> GetdetalleImpresion()
        {
            var detalleImpresion = await _context.fichaTecnica
                .ToListAsync();

            var detalleImpresionDto = _mapper.Map<List<DetalleImpresionDto>>(detalleImpresion);

            return Ok(detalleImpresionDto);
        }

        // GET: api/detalleImpresion/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleImpresionDto>> GetdetalleImpresion(int id)
        {
            var detalleImpresion = await _context.detalleImpresion
                .FirstOrDefaultAsync(u => u.idDetalleImpresion == id);

            var detalleImpresionDto = _mapper.Map<DetalleImpresionDto>(detalleImpresion);

            if (detalleImpresionDto == null)
            {
                return NotFound($"No se encontro el Detalle de Impresion con el ID: {id}");
            }

            return Ok(detalleImpresionDto);
        }

        // PUT: api/detalleImpresion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleImpresion(int id, UpdateDetalleImpresionDto updateDetalleImpresion)
        {
            var detalleImpresion = await _context.detalleImpresion.FindAsync(id);

            if (detalleImpresion == null)
            {
                return NotFound($"No se encontro el detalle de impresion con el ID {id}");
            }

            _mapper.Map(updateDetalleImpresion, detalleImpresion);
            _context.Entry(detalleImpresion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleImpresionExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleImpresion);
        }

        // POST: api/detalleImpresion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleImpresion>> PostdetalleImpresion(detalleImpresion detalleImpresion)
        {
            _context.detalleImpresion.Add(detalleImpresion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleImpresion", new { id = detalleImpresion.idDetalleImpresion }, detalleImpresion);
        }

        // DELETE: api/detalleImpresion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletedetalleImpresion(int id)
        {
            var detalleImpresion = await _context.detalleImpresion.FindAsync(id);
            if (detalleImpresion == null)
            {
                return NotFound();
            }

            _context.detalleImpresion.Remove(detalleImpresion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool detalleImpresionExists(int id)
        {
            return _context.detalleImpresion.Any(e => e.idDetalleImpresion == id);
        }
    }
}
