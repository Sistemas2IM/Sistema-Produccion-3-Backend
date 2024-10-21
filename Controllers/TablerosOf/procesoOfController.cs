using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class procesoOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public procesoOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOf()
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOf(int id)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el id: " + id);
            }

            return Ok(procesoOfDto);
        }

        // PUT: api/procesoOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutprocesoOf(int id, UpdateProcesoOfDto updateProcesoOf)
        {
            var procesoOf = await _context.procesoOf.FindAsync(id);

            if (procesoOf == null)
            {
                return NotFound("No se encontro el proceso de la of con el id: " + id);
            }

            _mapper.Map(updateProcesoOf, procesoOf);
            _context.Entry(procesoOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procesoOfExists(id))
                {
                    return NotFound("No se encontro el proceso de la of con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateProcesoOf);
        }

        // POST: api/procesoOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<procesoOf>> PostprocesoOf(procesoOf procesoOf)
        {
            _context.procesoOf.Add(procesoOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetprocesoOf", new { id = procesoOf.idProceso }, procesoOf);
        }

        // DELETE: api/procesoOf/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteprocesoOf(int id)
        {
            var procesoOf = await _context.procesoOf.FindAsync(id);
            if (procesoOf == null)
            {
                return NotFound();
            }

            _context.procesoOf.Remove(procesoOf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool procesoOfExists(int id)
        {
            return _context.procesoOf.Any(e => e.idProceso == id);
        }
    }
}
