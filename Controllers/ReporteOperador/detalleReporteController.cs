using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ReporteOperador
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleReporteController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleReporteController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleReporte - BUSCA TODOS LOS DETALLES EN BASE AL ID DE UN REPORTE DE OPERADOR
        [HttpGet("get/reporte/{idReporte}")]
        public async Task<ActionResult<IEnumerable<DetalleReporteDto>>> GetdetalleReporteByReporteId(string idReporte)
        {
            var detalleReporte = await _context.detalleReporte
                .Where(d => d.idReporte == idReporte) // Filtra por el id del reporte principal
                .Include(u => u.idOperacionNavigation)
                .Include(r => r.idMaterialNavigation)
                .Include(p => p.idTipoCierreNavigation)
                .Include(sm => sm.oFNavigation)
                .ToArrayAsync();

            if (detalleReporte == null || detalleReporte.Length == 0)
            {
                return NotFound("No se encontraron detalles relacionados con el reporte con id: " + idReporte);
            }

            var detalleReporteDto = _mapper.Map<List<DetalleReporteDto>>(detalleReporte);

            return Ok(detalleReporteDto);
        }

        // GET: api/detalleReporte/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleReporteDto>> GetdetalleReporte(int id)
        {
            var detalleReporte = await _context.detalleReporte
                .Include(u => u.idOperacionNavigation)
                .Include(r => r.idMaterialNavigation)
                .Include(p => p.idTipoCierreNavigation)
                .Include(sm => sm.oFNavigation)
                .FirstOrDefaultAsync(u => u.idDetalleReporte == id);

            var detalleReporteDto = _mapper.Map<DetalleReporteDto>(detalleReporte);

            if (detalleReporteDto == null)
            {
                return NotFound("No se encontro el detalle con el id: " + id);
            }

            return Ok(detalleReporteDto);
        }

        // PUT: api/detalleReporte/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleReporte(int id, UpdateDetalleReporteDto updateDetalleReporte)
        {
            var detalleReporte = await _context.detalleReporte.FindAsync(id);

            if (detalleReporte == null)
            {
                return BadRequest("No se encontro el detalle con el id: " + id);
            }

            _mapper.Map(updateDetalleReporte, detalleReporte);
            _context.Entry(detalleReporte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleReporteExists(id))
                {
                    return NotFound("No se encontro el detalle con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleReporte);
        }

        // POST: api/detalleReporte
        [HttpPost("post")]
        public async Task<ActionResult<detalleReporte>> PostdetalleReporte(string idReporte, AddDetalleReporteDto addDetalleReporte)
        {
            var reporteOperador = await _context.reportesDeOperadores.FindAsync(idReporte);

            if (reporteOperador == null)
            {
                return BadRequest("Invalid idReporte: " + idReporte);
            }

            var detalleReporte = _mapper.Map<detalleReporte>(addDetalleReporte);
            detalleReporte.idReporte = idReporte; // Set the relationship

            _context.detalleReporte.Add(detalleReporte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleReporte", new { id = detalleReporte.idDetalleReporte }, detalleReporte);
        }

        private bool detalleReporteExists(int id)
        {
            return _context.detalleReporte.Any(e => e.idDetalleReporte == id);
        }
    }
}
