using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.DetalleEntrega;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ProductoTerminado
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleEntregaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleEntregaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleEntrega
        [HttpGet("get/entrega/{idEntregaPt}")]
        public async Task<ActionResult<IEnumerable<DetalleEntregaDto>>> GetdetalleEntrega(int idEntregaPt)
        {
            var detalleEntrega = await _context.detalleEntrega
                .Where(d => d.idEntregaPt == idEntregaPt)
                .ToArrayAsync();

            if (detalleEntrega == null || detalleEntrega.Length == 0)
            {
                return NotFound("No se encontro detalles relacionados con el reporte con id: " + idEntregaPt);
            }

            var detalleEntregaDto = _mapper.Map<List<DetalleEntregaDto>>(detalleEntrega);

            return Ok(detalleEntregaDto);
        }

        // GET: api/detalleEntrega/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleEntregaDto>> GetdetalleEntregaId(int id)
        {
            var detalleEntrega = await _context.detalleEntrega.FindAsync(id);

            var detalleEntregaDto = _mapper.Map<DetalleEntregaDto>(detalleEntrega);

            if (detalleEntregaDto == null)
            {
                return NotFound("No se encuentra el detalle con el id: " + id);
            }

            return Ok(detalleEntregaDto);
        }

        // PUT: api/detalleEntrega/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleEntrega(int id, UpdateDetalleEntregaDto updateDetalleEntrega)
        {
            var detalleEntrega = await _context.detalleEntrega.FindAsync(id);

            if (detalleEntrega == null)
            {
                return NotFound("No se encontro el detalle con el id: " + id);
            }

            _mapper.Map(updateDetalleEntrega, detalleEntrega);
            _context.Entry(detalleEntrega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleEntregaExists(id))
                {
                    return NotFound("No se encontro el detalle con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleEntrega);
        }

        // POST: api/detalleEntrega
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleEntrega>> PostdetalleEntrega(AddDetalleEntregaDto addDetalleEntrega)
        {
       
            var detalleEntrega = _mapper.Map<detalleEntrega>(addDetalleEntrega);

            _context.detalleEntrega.Add(detalleEntrega);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleEntrega", new { id = detalleEntrega.idDetalleEntrega }, detalleEntrega);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddDetalleEntrega([FromBody] AddBatchDetalleDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.DetallesEntregas == null || !batchAddDto.DetallesEntregas.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var detalleEntregas = batchAddDto.DetallesEntregas.Select(dto => _mapper.Map<detalleEntrega>(dto)).ToList();

            await _context.detalleEntrega.AddRangeAsync(detalleEntregas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los detalles: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Detalles de la entrega agregados exitosamente.",
                DetallesAgregados = detalleEntregas
            });
        }

        private bool detalleEntregaExists(int id)
        {
            return _context.detalleEntrega.Any(e => e.idDetalleEntrega == id);
        }
    }
}
