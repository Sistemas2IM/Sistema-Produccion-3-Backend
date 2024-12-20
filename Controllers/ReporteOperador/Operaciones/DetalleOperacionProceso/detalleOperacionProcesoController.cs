using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones.DetalleOperacionProceso;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ReporteOperador.Operaciones.DetalleOperacionProceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleOperacionProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleOperacionProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleOperacionProceso
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<OperacionProcesoDto>>> GetdetalleOperacionProceso()
        {
            var operacionProceso = await _context.operaciones.ToListAsync();
            var operacionProcesoDto = _mapper.Map<List<OperacionProcesoDto>>(operacionProceso);

            return Ok(operacionProcesoDto);
        }

        // GET: api/detalleOperacionProceso POR ID DE REPORTE
        [HttpGet("get/idOperacion/{id}")]
        public async Task<ActionResult<IEnumerable<OperacionProcesoDto>>> GetdetalleOperacionProcesoIdOperacion(int id)
        {
            var operacionProceso = await _context.operaciones
                .Where(u => u.idOperacion == id)
                .ToListAsync();
            var operacionProcesoDto = _mapper.Map<List<OperacionProcesoDto>>(operacionProceso);

            return Ok(operacionProcesoDto);
        }

        // GET: api/detalleOperacionProceso/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<detalleOperacionProceso>> GetdetalleOperacionProceso(int id)
        {
            var detalleOperacionProceso = await _context.detalleOperacionProceso.FindAsync(id);
            var operacionProcesoDto = _mapper.Map<OperacionProcesoDto>(detalleOperacionProceso);

            if (detalleOperacionProceso == null)
            {
                return NotFound("No se encontro el registro con el ID: " + id);
            }

            return detalleOperacionProceso;
        }

        // PUT: api/detalleOperacionProceso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleOperacionProceso(int id, UpdateOperacionProcesoDto updateDetalleOperacionProceso)
        {
            var operacionProceso = await _context.detalleOperacionProceso.FindAsync(id);

            if (operacionProceso == null)
            {
                return NotFound("No se encontro el registro con el ID: " + id);
            }

            _mapper.Map(updateDetalleOperacionProceso, operacionProceso);
            _context.Entry(operacionProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleOperacionProcesoExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleOperacionProceso);
        }

        // POST: api/detalleOperacionProceso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleOperacionProceso>> PostdetalleOperacionProceso(AddOperacionProcesoDto addOperacionProceso)
        {
            var operacionProceso = _mapper.Map<detalleOperacionProceso>(addOperacionProceso);
            _context.detalleOperacionProceso.Add(operacionProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleOperacionProceso", new { id = operacionProceso.idDetalleOperacion }, operacionProceso);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddProcesoOf([FromBody] BatchAddOperacionProcesoDto batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchOperacionProcesos == null || !batchAddDto.addBatchOperacionProcesos.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var operacionProcesos = batchAddDto.addBatchOperacionProcesos.Select(dto => _mapper.Map<detalleOperacionProceso>(dto)).ToList();

            // Agregar los procesos a la base de datos
            await _context.detalleOperacionProceso.AddRangeAsync(operacionProcesos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar las opreaciones de procesos: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Operaciones de Procesos agregados exitosamente.",
                ProcesosAgregados = operacionProcesos
            });
        }

        private bool detalleOperacionProcesoExists(int id)
        {
            return _context.detalleOperacionProceso.Any(e => e.idDetalleOperacion == id);
        }
    }
}
