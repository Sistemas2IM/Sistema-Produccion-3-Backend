using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAPbobsCOM;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Impresoras;
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
                .Where(r => r.archivado == false || r.archivado == null) // Solo no archivados
                .OrderByDescending(f => f.fechaDeCreacion)
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
                .Include(o => o.operadorNavigation)
                .Select(m => new
                {
                    Reporte = m,
                    DetalleReporteOrdenado = m.detalleReporte
                        .OrderBy(d => d.horaInicio) // Ordena 'detalleReporte' por 'fechaDeCreacion'
                        .ToList()
                })
                .ToArrayAsync();

            var reporteOperadorDto = _mapper.Map<List<ReporteOperadorDto>>(reporteOperador.Select(r => r.Reporte).ToList());

            return Ok(reporteOperadorDto);
        }

        // GET: api/reportesDeOperadores por maquina
        [HttpGet("get/idMaquina/{id}")]
        public async Task<ActionResult<IEnumerable<ReporteOperadorDto>>> GetreportesDeOperadoresMaquina(int id)
        {
            var reporteOperador = await _context.reportesDeOperadores          
                .OrderByDescending(f => f.fechaDeCreacion)
                .Where(u => u.idMaquina == id && u.archivado == false || u.archivado == null) // Solo no archivados
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
                .Include(o => o.operadorNavigation)
                .Select(m => new
                {
                    Reporte = m,
                    DetalleReporteOrdenado = m.detalleReporte
                        .OrderBy(d => d.fechaHora ) // Ordena 'detalleReporte' por 'horaInicio' (ascendente)
                        .ToList()
                })
                .ToListAsync(); // Cambiado a ToListAsync para trabajar con la lista en memoria

            // Mapea el Reporte y asigna el DetalleReporteOrdenado
            var reporteOperadorDto = reporteOperador.Select(r =>
            {
                var reporteDto = _mapper.Map<ReporteOperadorDto>(r.Reporte);
                reporteDto.detalleReporte = _mapper.Map<List<DetalleReporteDto>>(r.DetalleReporteOrdenado);
                return reporteDto;
            }).ToList();

            return Ok(reporteOperadorDto);
        }

        // GET: api/reportesDeOperadores por maquina
        [HttpGet("get/Maquina/{id}/Operador/{user}")]
        public async Task<ActionResult<IEnumerable<ReporteOperadorDto>>> GetreportesDeOperadoresMaquinaBitacora(int id, string user)
        {
            var reporteOperador = await _context.reportesDeOperadores
                .OrderByDescending(f => f.fechaDeCreacion)
                .Where(u => u.idMaquina == id && u.operador == user && u.archivado == false || u.archivado == null)
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
                .Include(o => o.operadorNavigation)
                //.Select(m => new
                //{
                //    Reporte = m,
                //    DetalleReporteOrdenado = m.detalleReporte
                //    .OrderBy(d => d.horaInicio) // Ordena 'detalleReporte' por 'horaInicio' (ascendente)
                //    .ToList()
                //})
                .ToArrayAsync();

            var reporteOperadorDto = _mapper.Map<List<ReporteOperadorDto>>(reporteOperador);

            return Ok(reporteOperadorDto);
        }

        // GET: api/reportesDeOperadores por maquina
        [HttpGet("get/count/idMaquina/{id}")]
        public async Task<ActionResult<CountReporteOperadorDto>> GetreportesDeOperadoresCount(int id)
        {
            // Obtener el número de registros asociados al idMaquina
            var numeroReportes = await _context.reportesDeOperadores
                .Where(u => u.idMaquina == id)
                .CountAsync();

            // Crear el objeto DTO con el conteo incrementado
            var countReporteOperadorDto = new CountReporteOperadorDto
            {
                idMaquina = id,
                NumeroReportes = numeroReportes + 1 // Sumar +1 al conteo
            };

            return Ok(countReporteOperadorDto);
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
                .Include(o => o.operadorNavigation)
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

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateEstadosPT([FromBody] BatchUpdateReporteOperador batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateReporteOperador == null || !batchUpdateDto.updateReporteOperador.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateReporteOperador.Select(t => t.idReporte).ToList();

            // Obtener todos los roles relacionados
            var ept = await _context.reportesDeOperadores.Where(t => ids.Contains(t.idReporte)).ToListAsync();

            if (!ept.Any())
            {
                return NotFound("No se encontraron los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateReporteOperador)
            {
                var pt = ept.FirstOrDefault(t => t.idReporte == dto.idReporte);
                if (pt != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idEstadoReporte.HasValue)
                    {
                        pt.idEstadoReporte = dto.idEstadoReporte.Value;
                        pt.ultimaActualizacion = dto.ultimaActualizacion;
                        pt.actualizadoPor = dto.actualizadoPor;                       
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


        // POST: api/reportesDeOperadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost("post")]
        public async Task<ActionResult<reportesDeOperadores>> PostreportesDeOperadores(AddReporteOperadorDto addReporteOperador)
        {
            var reporteOperador = _mapper.Map<reportesDeOperadores>(addReporteOperador);
            _context.reportesDeOperadores.Add(reporteOperador);
            await _context.SaveChangesAsync();

            return Ok(addReporteOperador);
        }*/

        [HttpPost("post")]
        public async Task<ActionResult<reportesDeOperadores>> PostreportesDeOperadores(AddReporteOperadorDto addReporteOperador)
        {
            // Validar que el idMaquina sea válido
            if (addReporteOperador.idMaquina <= 0)
            {
                return BadRequest("El idMaquina es requerido y debe ser mayor que 0.");
            }

            // Obtener el nombreCorto de la máquina usando el idMaquina
            var maquina = await _context.maquinas
                .Where(m => m.idMaquina == addReporteOperador.idMaquina)
                .Select(m => new { m.nombreCorto })
                .FirstOrDefaultAsync();

            if (maquina == null)
            {
                return NotFound("No se encontró la máquina con el idMaquina especificado.");
            }

            // Obtener el número de reportes existentes para la máquina especificada (basado en idMaquina)
            int conteoReportes = await _context.reportesDeOperadores
                .CountAsync(r => r.idMaquina == addReporteOperador.idMaquina);

            // Generar el nuevo número de reporte
            int nuevoNumeroReporte = conteoReportes + 1;

            // Generar el nuevo idReporte
            string nuevoIdReporte = $"{maquina.nombreCorto}_{nuevoNumeroReporte.ToString().PadLeft(6, '0')}";

            // Verificar si el nuevo idReporte ya existe (por si acaso)
            var reporteExistente = await _context.reportesDeOperadores
                .AnyAsync(r => r.idReporte == nuevoIdReporte);

            if (reporteExistente)
            {
                return Conflict("El idReporte generado ya existe en la base de datos.");
            }

            // Mapear el DTO a la entidad
            var reporteOperador = _mapper.Map<reportesDeOperadores>(addReporteOperador);

            // Asignar el nuevo idReporte
            reporteOperador.idReporte = nuevoIdReporte;

            // Guardar el nuevo reporte
            _context.reportesDeOperadores.Add(reporteOperador);
            await _context.SaveChangesAsync();

            return Ok(reporteOperador);
        }

        private bool reportesDeOperadoresExists(string id)
        {
            return _context.reportesDeOperadores.Any(e => e.idReporte == id);
        }
    }
}
