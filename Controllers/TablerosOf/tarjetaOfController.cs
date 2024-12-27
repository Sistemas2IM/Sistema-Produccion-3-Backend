using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.ApiKey;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.Reportes;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class tarjetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tarjetaOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tarjetaOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOf()
        {
            var tarjetaOf = await _context.tarjetaOf
                .OrderBy(p => p.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }

        // GET: api/tarjetaOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TarjetaOfDto>> GettarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .FirstOrDefaultAsync(u => u.oF == id);
            var tarjetaOfDto = _mapper.Map<TarjetaOfDto>(tarjetaOf);
            
            if (tarjetaOfDto == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            return Ok(tarjetaOfDto);
        }

        // PUT: api/tarjetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttarjetaOf(int id, UpdateTarjetaOfDto updateTarjetaOf)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);

            if (tarjetaOf == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            _mapper.Map(updateTarjetaOf, tarjetaOf);
            _context.Entry(tarjetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tarjetaOfExists(id))
                {
                    return NotFound("No se encontro la tarjeta con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTarjetaOf);
        }

        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateTarjetas([FromBody] BatchUpdatePosicionTarjetaOfDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.Tarjetas == null || !batchUpdateDto.Tarjetas.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.Tarjetas.Select(t => t.oF).ToList();

            // Obtener todas las tarjetas relacionadas
            var tarjetas = await _context.tarjetaOf.Where(t => ids.Contains(t.oF)).ToListAsync();

            if (!tarjetas.Any())
            {
                return NotFound("No se encontraron tarjetas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.Tarjetas)
            {
                var tarjeta = tarjetas.FirstOrDefault(t => t.oF == dto.oF);
                if (tarjeta != null)
                {
                    // Actualizar la posición si es proporcionada
                    if (dto.posicion.HasValue)
                    {
                        tarjeta.posicion = dto.posicion.Value;
                    }

                    _context.Entry(tarjeta).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las tarjetas.");
            }

            return Ok("Actualización realizada correctamente.");
        }


        // POST: api/tarjetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tarjetaOf>> PosttarjetaOf(AddTarjetaOfDto addTarjetaOf)
        {
            var tarjetaOf = _mapper.Map<tarjetaOf>(addTarjetaOf);

            _context.tarjetaOf.Add(tarjetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarjetaOf", new { id = tarjetaOf.oF }, tarjetaOf);
        }


        // DASHBOARD =========================================================================

        // GET: api/tarjetaOf/lineaNegocio
        [HttpGet("get/lineaNegocio")]
        public async Task<ActionResult<IEnumerable<object>>> GetTarjetaOfCountByLineaNegocio()
        {
            var tarjetaOf = await _context.tarjetaOf.ToListAsync();

            // Agrupar las tarjetas por línea de negocio y contar cada grupo
            var tarjetaOfGrouped = tarjetaOf.GroupBy(t => t.lineaDeNegocio)
                                            .Select(group => new {
                                                LineaNegocio = group.Key,
                                                CantidadTarjetas = group.Count()
                                            });

            return Ok(tarjetaOfGrouped);
        }

        // GET: api/tarjetaOf/vencidas
        [HttpGet("get/vencidas")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOfVencidas()
        {
            var tarjetaOf = await _context.tarjetaOf
                .Where(d => d.fechaVencimiento < DateTime.Today)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }

        // GET: api/tarjetaOf/cerradasHoy
        [HttpGet("get/cerradasHoy")]
        public async Task<ActionResult<int>> GetTarjetaOfCerradasHoy()
        {
            var today = DateTime.Now.Date;

            // Contar las tarjetas que se cerraron hoy y tienen el estado cerrado (idEstadoOf == 4)
            var cantidadTarjetasCerradasHoy = await _context.tarjetaOf
                .Where(d => d.finalizacion.HasValue && d.finalizacion.Value.Date == today && d.idEstadoOf == 4)
                .CountAsync();

            return Ok(new { CantidadTarjetasCerradasHoy = cantidadTarjetasCerradasHoy });
        }


        // - REPORTES - =======================================================================================

        // GET: api/tarjetaOf
        [HttpGet("get/PM")]
        public async Task<ActionResult<IEnumerable<ReportePMTarjetaOf>>> GettarjetaOfPM()
        {
            var tarjetas = await _context.tarjetaOf
                .OrderBy(p => p.vendedorOf)
                .ToListAsync();

            var tarjetasDto = _mapper.Map<List<ReportePMTarjetaOf>>(tarjetas);

            // Calcular días en planta y días a la entrega
            foreach (var tarjeta in tarjetasDto)
            {
                if (tarjeta.fechaCreacion.HasValue)
                {
                    tarjeta.diasEnPlanta = (DateTime.Now - tarjeta.fechaCreacion.Value).Days;
                }

                if (tarjeta.fechaVencimiento.HasValue)
                {
                    tarjeta.diasALaEntrega = (tarjeta.fechaVencimiento.Value - DateTime.Now).Days;
                }
            }

            return Ok(tarjetasDto);
        }


        private bool tarjetaOfExists(int id)
        {
            return _context.tarjetaOf.Any(e => e.oF == id);
        }
    }
}
