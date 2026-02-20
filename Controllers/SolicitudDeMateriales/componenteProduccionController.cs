using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class componenteProduccionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public componenteProduccionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<componenteProduccionController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ComponenteProduccionDto>>> etComponenteProduccion()
        {
            var componentes = await _context.componenteProduccion.ToListAsync();

            var componentesDto = _mapper.Map<List<ComponenteProduccionDto>>(componentes);

            return Ok(componentesDto);
        }

        // GET api/<componenteProduccionController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<ComponenteProduccionDto>>> GetComponenteProduccionId(int id)
        {
            var componente = await _context.componenteProduccion
                .FindAsync(id);

            var componenteDto = _mapper.Map<ComponenteProduccionDto>(componente);

            if (componenteDto == null)
            {
                return NotFound();
            }

            return Ok(componenteDto);
        }

        [HttpGet("get/procesoOf/{id}")]
        public async Task<ActionResult<IEnumerable<ComponenteProduccionDto>>> etComponenteProduccionProceso(int id)
        {
            var componentes = await _context.componenteProduccion
                .Where(c => c.idProceso == id)
                .ToListAsync();

            var componentesDto = _mapper.Map<List<ComponenteProduccionDto>>(componentes);

            return Ok(componentesDto);
        }

        // POST api/<componenteProduccionController>
        [HttpPost("post")]
        public async Task<ActionResult<componenteProduccion>> PostComponente(AddComponenteProduccionDto componenteDto)
        {
            var componente = _mapper.Map<componenteProduccion>(componenteDto);
            _context.componenteProduccion.Add(componente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponenteProduccionId", new { id = componente.idComponente }, componente);
        }

        // POST BATCH api/<componenteProduccionController>/batch
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddComponente([FromBody] BatchAddComponenteProd batchComponente)
        {
            if (batchComponente.componentes == null || !batchComponente.componentes.Any())
            {
                return BadRequest("No se proporcionaron componentes para agregar.");
            }
            
            var componentes = batchComponente.componentes.Select(c => _mapper.Map<componenteProduccion>(c)).ToList();

            await _context.componenteProduccion.AddRangeAsync(componentes);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar los componentes: {ex.Message}");
            }

            return Ok(new { message = "Componentes agregados exitosamente", componenteProduccion = componentes});
        }

        // PUT api/<componenteProduccionController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutComponente(int id, UpdateComponenteProduccionDto componenteDto)
        {
            var componente = await _context.componenteProduccion.FindAsync(id);
            if (componente == null)
            {
                return NotFound();
            }

            _mapper.Map(componenteDto, componente);
            _context.Entry(componente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponenteProduccionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(componenteDto);
        }

        // PUT BATCH
        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateCom([FromBody] BatchUpdateComponenteProd batchComponente)
        {
            if (batchComponente == null || batchComponente.componentes == null || !batchComponente.componentes.Any())
            {
                return BadRequest("No se proporcionaron componentes para actualizar.");
            }

            var ids = batchComponente.componentes.Select(c => c.idComponente).ToList();
            var componentesExistentes = await _context.componenteProduccion
                .Where(c => ids.Contains(c.idComponente))
                .ToListAsync();

            if (!componentesExistentes.Any())
            {
                return NotFound("No se encontraron componentes con los IDs proporcionados.");
            }

            foreach (var dto in batchComponente.componentes)
            {
                var componente = componentesExistentes.FirstOrDefault(c => c.idComponente == dto.idComponente);
                if (componente != null)
                {
                    _mapper.Map(dto, componente);

                    _context.Entry(componente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar los componentes");
            }

            return Ok(new { message = "Componentes actualizados exitosamente", componenteProduccion = componentesExistentes });
        }

        // DELTE BATCH
        [HttpDelete("delete/BatchDelete")]
        public async Task<IActionResult> DeleteComponente([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No se proporcionaron IDs para eliminar.");
            }

            var componentes = await _context.componenteProduccion
                .Where(c => ids.Contains(c.idComponente))
                .ToListAsync();

            if (!componentes.Any())
            {
                return NotFound("No se encontraron componentes con los IDs proporcionados.");
            }

            _context.componenteProduccion.RemoveRange(componentes);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar los componentes: {ex.Message}");
            }
            return Ok(new { message = "Componentes eliminados exitosamente", componenteProduccion = componentes });
        }


        private bool ComponenteProduccionExists(int id)
        {
            return _context.componenteProduccion.Any(e => e.idComponente == id);
        }
    }
}
