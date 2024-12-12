using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class procesoOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public procesoOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOf()
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOf(int id)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el id: " + id);
            }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5
        [HttpGet("get/oF/{of}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOfTarjeta(int of)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .FirstOrDefaultAsync(u => u.oF == of);

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el numero de of: " + of);
            }

            return Ok(procesoOfDto);
        }

        // PUT: api/procesoOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutprocesoOf(int id, UpdateProcesoOfDto updateProcesoOf)
        {
            var procesoOf = await _context.procesoOf.FindAsync(id);

            if (procesoOf == null)
            {
                return NotFound("No se encontro el proceso de la of con el id: " + id);
            }

            _mapper.Map(updateProcesoOf, procesoOf);
            _context.Entry(procesoOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procesoOfExists(id))
                {
                    return NotFound("No se encontro el proceso de la of con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateProcesoOf);
        }

        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateProcesos([FromBody] BatchUpdateProcesoOfDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.ProcesosOf == null || !batchUpdateDto.ProcesosOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.ProcesosOf.Select(t => t.oF).ToList();

            // Obtener todas los procesos of relacionadas
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.oF)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.ProcesosOf)
            {
                var proceso = procesos.FirstOrDefault(t => t.oF == dto.oF);
                if (proceso != null)
                {
                    // Actualizar la posición si es proporcionada
                    if (dto.posicion.HasValue)
                    {
                        proceso.posicion = dto.posicion.Value;
                    }

                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        // POST: api/procesoOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<procesoOf>> PostprocesoOf(AddProcesoOfDto addProcesoOf)
        {
            var procesoOf = _mapper.Map<procesoOf>(addProcesoOf);
            _context.procesoOf.Add(procesoOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetprocesoOf", new { id = procesoOf.idProceso }, procesoOf);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddProcesoOf([FromBody] BatchAddProcesoOf batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchProcesoDto == null || !batchAddDto.addBatchProcesoDto.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var procesos = batchAddDto.addBatchProcesoDto.Select(dto => _mapper.Map<procesoOf>(dto)).ToList();

            // Agregar los procesos a la base de datos
            await _context.procesoOf.AddRangeAsync(procesos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los procesos: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Procesos agregados exitosamente.",
                ProcesosAgregados = procesos
            });
        }


        private bool procesoOfExists(int id)
        {
            return _context.procesoOf.Any(e => e.idProceso == id);
        }
    }
}
