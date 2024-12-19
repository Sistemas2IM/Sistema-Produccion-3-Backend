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
    public class reportesDeOperadoresController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public reportesDeOperadoresController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/reportesDeOperadores
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ReporteOperadorDto>>> GetreportesDeOperadores()
        {
            var reporteOperador = await _context.reportesDeOperadores
                .Include(r => r.idEstadoReporteNavigation)
                .Include(p => p.idMaquinaNavigation)
                .Include(sm => sm.idTipoReporteNavigation)
                .Include(m => m.detalleReporte) // Incluye 'detalleReporte'
                    .ThenInclude(d => d.idOperacionNavigation) // Incluye la relación con 'idOperacion'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idMaterialNavigation) // Incluye la relación con 'idMaterial'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idTipoCierreNavigation) // Incluye la relación con 'idTipoCierre'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.oFNavigation) // Incluye la relación con 'idTarjetaOf'
                .ToArrayAsync();
            
            var reporteOperadorDto = _mapper.Map<List<ReporteOperadorDto>>(reporteOperador);

            return Ok(reporteOperadorDto);
        }

        // GET: api/reportesDeOperadores por maquina
        [HttpGet("get/idMaquina/{id}")]
        public async Task<ActionResult<IEnumerable<ReporteOperadorDto>>> GetreportesDeOperadoresMaquina(int id)
        {
            var reporteOperador = await _context.reportesDeOperadores
                .Where(u => u.idMaquina == id)
                .Include(r => r.idEstadoReporteNavigation)
                .Include(p => p.idMaquinaNavigation)
                .Include(sm => sm.idTipoReporteNavigation)
                .Include(m => m.detalleReporte) // Incluye 'detalleReporte'
                    .ThenInclude(d => d.idOperacionNavigation) // Incluye la relación con 'idOperacion'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idMaterialNavigation) // Incluye la relación con 'idMaterial'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idTipoCierreNavigation) // Incluye la relación con 'idTipoCierre'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.oFNavigation) // Incluye la relación con 'idTarjetaOf'
                .ToArrayAsync();

            var reporteOperadorDto = _mapper.Map<List<ReporteOperadorDto>>(reporteOperador);

            return Ok(reporteOperadorDto);
        }

        // GET: api/reportesDeOperadores/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ReporteOperadorDto>> GetreportesDeOperadores(string id)
        {
            var reporteOperador = await _context.reportesDeOperadores
                .Include(r => r.idEstadoReporteNavigation)
                .Include(p => p.idMaquinaNavigation)
                .Include(sm => sm.idTipoReporteNavigation)
                .Include(m => m.detalleReporte) // Incluye 'detalleReporte'
                    .ThenInclude(d => d.idOperacionNavigation) // Incluye la relación con 'idOperacion'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idMaterialNavigation) // Incluye la relación con 'idMaterial'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.idTipoCierreNavigation) // Incluye la relación con 'idTipoCierre'
                .Include(m => m.detalleReporte)
                    .ThenInclude(d => d.oFNavigation) // Incluye la relación con 'idTarjetaOf'
                .FirstOrDefaultAsync(u => u.idReporte == id);

            if (reporteOperador == null)
            {
                return NotFound("No se encontro el reporte con el id: " + id);
            }

            var reporteOperadorDto = _mapper.Map<ReporteOperadorDto>(reporteOperador);

            return Ok(reporteOperadorDto);
        }

        // PUT: api/reportesDeOperadores/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutreportesDeOperadores(string id, UpdateReporteOperadorDto updateReporteOperador)
        {
            var reporteOperador = await _context.reportesDeOperadores.FindAsync(id);

            if (reporteOperador == null)
            {
                return BadRequest("Id no valido: " + id);
            }

            _mapper.Map(updateReporteOperador, reporteOperador);
            _context.Entry(reporteOperador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!reportesDeOperadoresExists(id))
                {
                    return NotFound("No se encontro el reporte con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateReporteOperador);
        }

        // POST: api/reportesDeOperadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<reportesDeOperadores>> PostreportesDeOperadores(AddReporteOperadorDto addReporteOperador)
        {
            var reporteOperador = _mapper.Map<reportesDeOperadores>(addReporteOperador);

            _context.reportesDeOperadores.Add(reporteOperador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReporte", new { id = reporteOperador.idReporte }, reporteOperador);
        }
    
        private bool reportesDeOperadoresExists(string id)
        {
            return _context.reportesDeOperadores.Any(e => e.idReporte == id);
        }
    }
}
