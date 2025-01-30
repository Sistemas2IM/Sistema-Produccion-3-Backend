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
                .Include(o => o.ofNavigation)
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
                .Include(o => o.ofNavigation)
                .FirstOrDefaultAsync(u => u.idEntregaPt == id);

            if (entregasProductoTerminado == null)
            {
                return NotFound("No se cnontro el reporte con el id: " + id);
            }

            var productoTerminado = _mapper.Map<ProductoTerminadoDto>(entregasProductoTerminado);

            return Ok(productoTerminado);
        }

        [HttpGet("get/ultimoPT")]
        public async Task<ActionResult<int>> GetentregasProductoTerminadoUltimo()
        {
            var ultimoId = await _context.entregasProductoTerminado
                .OrderByDescending(u => u.idEntregaPt)
                .Select(u => u.idEntregaPt)
                .FirstOrDefaultAsync();

            // Incrementar en 1
            var siguienteId = ultimoId + 1;

            // Devolver el siguiente ID a generar xd
            return Ok(siguienteId);
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

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateEstadosPT([FromBody] BatchUpdateProductoTerminado batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchProductoTerminado == null || !batchUpdateDto.updateBatchProductoTerminado.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchProductoTerminado.Select(t => t.idEntregaPt).ToList();

            // Obtener todos los roles relacionados
            var ept = await _context.entregasProductoTerminado.Where(t => ids.Contains(t.idEntregaPt)).ToListAsync();

            if (!ept.Any())
            {
                return NotFound("No se encontraron los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchProductoTerminado)
            {
                var pt = ept.FirstOrDefault(t => t.idEntregaPt == dto.idEntregaPt);
                if (pt != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idEstadoReporte.HasValue)
                    {
                        pt.idEstadoReporte = dto.idEstadoReporte.Value;
                        pt.recibidoPor = dto.recibidoPor;
                        pt.fechaRecepcion = dto.fechaRecepcion;
                        pt.actualizadoPor = dto.actualizadoPor;
                        pt.ultimaActualizacion = dto.ultimaActualizacion;
                    }

                    _context.Entry(pt).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los estados.");
            }

            return Ok("Actualización realizada correctamente.");
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
