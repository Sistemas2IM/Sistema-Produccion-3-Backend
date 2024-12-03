using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleBarnizController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleBarnizController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleBarniz
        [HttpGet("get/DetalleImpresion/{id}")]
        public async Task<ActionResult<IEnumerable<DetalleBarnizDto>>> GetdetalleBarnizImpresion(int id)
        {
            var detalleBarniz = await _context.detalleBarniz
                .Where(d => d.idDetalleImpresion == id)
                .Include(u => u.potenciaLamparaUv)
                .ToListAsync();

            var detalleBarnizDto = _mapper.Map<List<DetalleBarnizDto>>(detalleBarniz);

            return Ok(detalleBarnizDto);
        }

        // GET: api/detalleBarniz/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleBarnizDto>> GetdetalleBarniz(int id)
        {
            var detalleBarniz = await _context.detalleBarniz
                .Include(u => u.potenciaLamparaUv)
                .FirstOrDefaultAsync(u => u.idDetalleImpresion == id);

            var detalleBarnizDto = _mapper.Map<DetalleBarnizDto>(detalleBarniz);

            if (detalleBarnizDto == null)
            {
                return NotFound($"No se encontro el detalle de barniz con el ID: {id}");
            }

            return Ok(detalleBarnizDto);
        }

        // PUT: api/detalleBarniz/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleBarniz(int id, UpdateDetalleBarnizDto updateDetalleBarniz)
        {
            var detalleBarniz = await _context.detalleBarniz.FindAsync(id);

            if (detalleBarniz == null)
            {
                return NotFound($"No se encontro el detalle de barniz con el ID: {id}");
            }

            _mapper.Map(updateDetalleBarniz, detalleBarniz);
            _context.Entry(detalleBarniz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleBarnizExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleBarniz);
        }

        // POST: api/detalleBarniz
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleBarniz>> PostdetalleBarniz(AddDetalleBarnizDto addDetalleBarniz)
        {
            var detalleBarniz = _mapper.Map<detalleBarniz>(addDetalleBarniz);
            _context.detalleBarniz.Add(detalleBarniz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleBarniz", new { id = detalleBarniz.idBarniz }, detalleBarniz);
        }

        private bool detalleBarnizExists(int id)
        {
            return _context.detalleBarniz.Any(e => e.idBarniz == id);
        }
    }
}
