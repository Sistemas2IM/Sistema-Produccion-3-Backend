using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Impresoras;
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
                .OrderBy(h => h.horaInicio)
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

        // PUT BATCH
        [HttpPut("put/BatchUpdateImpresora")]
        public async Task<IActionResult> BatchUpdateDetalleImpre([FromBody] BatchUpdateDetalleImpresion batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateBatchImpresoras == null || !batchUpdateDto.updateBatchImpresoras.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateBatchImpresoras.Select(t => t.idDetalleReporte).ToList();

            // Obtener todos los roles relacionados
            var detalles = await _context.detalleReporte.Where(t => ids.Contains(t.idDetalleReporte)).ToListAsync();

            if (!detalles.Any())
            {
                return NotFound("No se encontraron detalles para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateBatchImpresoras)
            {
                var detalle = detalles.FirstOrDefault(t => t.idDetalleReporte == dto.idDetalleReporte);
                if (detalle != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idDetalleReporte != 0)
                    {
                        detalle.idOperacion = dto.idOperacion;
                        detalle.oF = dto.oF;
                        detalle.numeroFila = dto.numeroFila;
                        detalle.horaFinal = dto.horaFinal;
                        detalle.horaInicio = dto.horaInicio;
                        detalle.tiempo = dto.tiempo;
                        detalle.descripcion = dto.descripcion;
                        detalle.cliente = dto.cliente;
                        detalle.tiroRetiro = dto.tiroRetiro;
                        detalle.cantidadRecibida = dto.cantidadRecibida;
                        detalle.cantidadProducida = dto.cantidadProducida;
                        detalle.cantidadDanada = dto.cantidadDanada;
                        detalle.cantidadSolicitada = dto.cantidadSolicitada;
                        detalle.cantidadNc = dto.cantidadNc;
                        detalle.observaciones = dto.observaciones;
                        detalle.accionPorAuxiliar = dto.accionPorAuxiliar;
                        detalle.anchoBobina = dto.anchoBobina;
                        detalle.velocidadMaquina = dto.velocidadMaquina;
                        detalle.largoConvertido = dto.largoConvertido;
                        detalle.bjAncho = dto.bjAncho;
                        detalle.bjLargo = dto.bjLargo;
                        detalle.bsAncho = dto.bsAncho;
                        detalle.bsLargo = dto.bsLargo;
                        detalle.ancho = dto.ancho;
                        detalle.alto = dto.alto;
                        detalle.repeticiones = dto.repeticiones;
                        detalle.cantidadSobrante = dto.cantidadSobrante;
                        detalle.udCorrugados = dto.udCorrugados;
                        detalle.fechaHora = dto.fechaHora;
                    }

                    _context.Entry(detalle).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los detalles.");
            }

            return Ok("Actualización realizada correctamente.");
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

        // POST: BATCH
        [HttpPost("post/BatchAddImpresora")]
        public async Task<IActionResult> BatchAddDetalleReporteOperador([FromBody] BatchAddDetalleImpresora batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.batchImpresoras == null || !batchAddDto.batchImpresoras.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var detalleReportes = batchAddDto.batchImpresoras.Select(dto => _mapper.Map<detalleReporte>(dto)).ToList();

            // Agregar los procesos a la base de datos
            await _context.detalleReporte.AddRangeAsync(detalleReportes);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los detalles: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "detalles agregados exitosamente.",
                ProcesosAgregados = detalleReportes
            });
        }
        private bool detalleReporteExists(int id)
        {
            return _context.detalleReporte.Any(e => e.idDetalleReporte == id);
        }
    }
}
