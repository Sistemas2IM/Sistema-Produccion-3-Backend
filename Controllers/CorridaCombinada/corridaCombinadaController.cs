using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf.BatchEtiquetaOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.CorridaCombinada
{
    [Route("api/[controller]")]
    [ApiController]
    public class corridaCombinadaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public corridaCombinadaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CorridaCombinadaDto>>> GetCorridaCombinada()
        {
            var corridas = await _context.corridaCombinada
                .ToListAsync();
            var corridasDto = _mapper.Map<List<CorridaCombinadaDto>>(corridas);
            return Ok(corridasDto);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<CorridaCombinadaDto>> GetCorridaCombinada(int id)
        {
            var corrida = await _context.corridaCombinada
                .FirstOrDefaultAsync(c => c.idRelacion == id);
            if (corrida == null)
            {
                return NotFound();
            }
            var corridaDto = _mapper.Map<CorridaCombinadaDto>(corrida);
            return Ok(corridaDto);
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCorridaCombinada(int id, UpdateCorridaCombinadaDto corridaDto)
        {
            var corrida = await _context.corridaCombinada.FindAsync(id);
            if (corrida == null)
            {
                return NotFound();
            }
            _mapper.Map(corridaDto, corrida);
            _context.Entry(corrida).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorridaCombinadaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(corridaDto);
        }

        [HttpPost("post")]
        public async Task<ActionResult<CorridaCombinadaDto>> PostCorridaCombinada(AddCorridaCombinadaDto corridaDto)
        {
            var corrida = _mapper.Map<corridaCombinada>(corridaDto);
            _context.corridaCombinada.Add(corrida);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCorridaCombinada", new { id = corrida.idRelacion }, corridaDto);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddTarjetaEtiqueta([FromBody] BatchAdddCorridaCombinada batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchCorridaCombinada == null || !batchAddDto.addBatchCorridaCombinada.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            var corridaCombinada = batchAddDto.addBatchCorridaCombinada.Select(dto => _mapper.Map<corridaCombinada>(dto)).ToList();

            await _context.corridaCombinada.AddRangeAsync(corridaCombinada);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar las etiquetas: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Cambios agregados exitosamente.",
                etiquetas = corridaCombinada
            });
        }

        private bool CorridaCombinadaExists(int id)
        {
            return _context.corridaCombinada.Any(e => e.idRelacion == id);
        }
    }
}
