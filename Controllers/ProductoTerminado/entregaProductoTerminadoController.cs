using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ProductoTerminado
{
    [Route("api/[controller]")]
    [ApiController]
    public class entregaProductoTerminadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public entregaProductoTerminadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/entregaProductoTerminado
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ProductoTerminadoDto>>> GetentregasProductoTerminado()
        {
            var productoTerminado = await _context.entregasProductoTerminado
                .Include(u => u.contenidoEntrega)
                .Include(r => r.detalleEntrega)
                .Include(p => p.idEstadoReporteNavigation)
                .Include(sm => sm.idMaquinaNavigation)
                .Include(m => m.oVNavigation)
                .Include(d => d.oFNavigation)
                .ToListAsync();

            var productoTerminadoDto = _mapper.Map<List<ProductoTerminadoDto>>(productoTerminado);

            return Ok(productoTerminadoDto);
        }

        // GET: api/entregaProductoTerminado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProductoTerminadoDto>> GetentregasProductoTerminado(int id)
        {
            var entregasProductoTerminado = await _context.entregasProductoTerminado
                .Include(u => u.contenidoEntrega)
                .Include(r => r.detalleEntrega)
                .Include(p => p.idEstadoReporteNavigation)
                .Include(sm => sm.idMaquinaNavigation)
                .Include(m => m.oVNavigation)
                .Include(d => d.oFNavigation)
                .FirstOrDefaultAsync(u => u.idEntregaPt == id);

            if (entregasProductoTerminado == null)
            {
                return NotFound("No se cnontro el reporte con el id: " + id);
            }

            var productoTerminado = _mapper.Map<ProductoTerminadoDto>(entregasProductoTerminado);

            return Ok(productoTerminado);
        }

        // PUT: api/entregaProductoTerminado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutentregasProductoTerminado(int id, UpdateProductoTerminadoDto updateProductoTerminado)
        {
            var productoTerminado = await _context.entregasProductoTerminado.FindAsync(id);

            if (productoTerminado ==  null)
            {
                return BadRequest("Id no valido: " + id);
            }

            _mapper.Map(updateProductoTerminado, productoTerminado);
            _context.Entry(productoTerminado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!entregasProductoTerminadoExists(id))
                {
                    return NotFound("No se encontro el reporte con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateProductoTerminado);
        }

        // POST: api/entregaProductoTerminado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<entregasProductoTerminado>> PostentregasProductoTerminado(AddProductoTerminadoDto addProductoTerminado)
        {
            var productoTerminado = _mapper.Map<entregasProductoTerminado>(addProductoTerminado);

            _context.entregasProductoTerminado.Add(productoTerminado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetentregasProductoTerminado", new { id = productoTerminado.idEntregaPt }, productoTerminado);
        }

        private bool entregasProductoTerminadoExists(int id)
        {
            return _context.entregasProductoTerminado.Any(e => e.idEntregaPt == id);
        }
    }
}
