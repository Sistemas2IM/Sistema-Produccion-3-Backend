using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones.DetalleOperacionProceso;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones.DetalleOperacionProceso.DetalleOperacionProcesoOF;
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
            var operacionProceso = await _context.detalleOperacionProceso.ToListAsync();
            var operacionProcesoDto = _mapper.Map<List<OperacionProcesoDto>>(operacionProceso);

            return Ok(operacionProcesoDto);
        }

        // GET: api/detalleOperacionProceso POR ID DE REPORTE
        [HttpGet("get/idOperacion/{id}")]
        public async Task<ActionResult<IEnumerable<OperacionProcesoDto>>> GetdetalleOperacionProcesoIdOperacion(int id)
        {
            var operacionProceso = await _context.detalleOperacionProceso
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

        // GET: api/detalleOperacionProceso
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<OperacionProcesoDto>>> GetdetalleOperacionProcesoOf(int of)
        {
            var operacionProceso = await _context.detalleOperacionProceso               
                .Include(o => o.idProcesoNavigation)
                .Include(m => m.maquinaNavigation)
                .Include(o => o.idOperacionNavigation)
                .Where(o => o.idProcesoNavigation.oF == of)
                .ToListAsync();
            var operacionProcesoDto = _mapper.Map<List<OperacionProcesoDto>>(operacionProceso);

            return Ok(operacionProcesoDto);
        }

        // GET: api/detalleOperacionProceso/5
        [HttpGet("get/{ofId}/{idMaquina}")]
        public async Task<ActionResult<IEnumerable<OperacionProcesoOfMaquina>>> GetOperacionProceso(int ofId, int idMaquina)
        {
            // Realizar el JOIN y filtrar según los parámetros
            var query = from t0 in _context.tarjetaOf
                        join t1 in _context.procesoOf on t0.oF equals t1.oF
                        join t2 in _context.detalleOperacionProceso on t1.idProceso equals t2.idProceso
                        join t3 in _context.tablerosOf on t1.idTablero equals t3.idTablero
                        where t0.oF == ofId && t3.idMaquina == idMaquina
                        select new OperacionProcesoOfMaquina
                        {
                            OF = t0.oF,
                            ClienteOf = t0.clienteOf,
                            ProductoOf = t0.productoOf,
                            Inicio = t2.inicio,
                            Finalizacion = t2.finalizacion,
                            IdOperacion = t2.idOperacion
                        };

            var result = await query.ToListAsync();

            if (!result.Any())
            {
                return NotFound("No se encontraron registros con los parámetros proporcionados.");
            }

            return Ok(result);
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

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateEstadosPT([FromBody] BatchUpdateOperacionProceso batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateOperacionProcesos == null || !batchUpdateDto.updateOperacionProcesos.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateOperacionProcesos.Select(t => t.idDetalleOperacion).ToList();

            // Obtener todos los roles relacionados
            var ept = await _context.detalleOperacionProceso.Where(t => ids.Contains(t.idDetalleOperacion)).ToListAsync();

            if (!ept.Any())
            {
                return NotFound("No se encontraron los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateOperacionProcesos)
            {
                var pt = ept.FirstOrDefault(t => t.idDetalleOperacion == dto.idDetalleOperacion);
                if (pt != null)
                {
                    // Actualizar propiedades específicas
                    if (dto.idDetalleOperacion > 0)
                    {
                        pt.numeroFila = dto.numeroFila;
                        pt.secuencia = dto.secuencia;
                        pt.inicio = dto.inicio;
                        pt.finalizacion = dto.finalizacion;
                        pt.operador = dto.operador;
                        pt.idOperacion = dto.idOperacion;
                        pt.accionPorAuxiliar = dto.accionPorAuxiliar;
                        pt.auxiliar = dto.auxiliar;
                        pt.cantidadRecibida = dto.cantidadRecibida;
                        pt.cantidadProducida = dto.cantidadProducida;
                        pt.cantidadNc = dto.cantidadNc;
                        pt.tiroRetiro = dto.tiroRetiro;
                        pt.fechaHora = dto.fechaHora;
                        pt.bjAncho = dto.bjAncho;
                        pt.bjLargo = dto.bjLargo;
                        pt.largoConvertido = dto.largoConvertido;
                        pt.bsLargo = dto.bsLargo;
                        pt.bsAncho = dto.bsAncho;
                        pt.anchoBobina = dto.anchoBobina;
                        pt.cantAjuste = dto.cantAjuste;
                        pt.cantProducir = dto.cantProducir;
                        pt.cantSolicitada = dto.cantSolicitada;
                        pt.cantProducida = dto.cantProducida;
                        pt.maquina = dto.maquina;
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
