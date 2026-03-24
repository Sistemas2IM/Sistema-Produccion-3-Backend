using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FormulacionTinta
{
    [Route("api/[controller]")]
    [ApiController]
    public class formulacionTintaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public formulacionTintaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<formulacionTintaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FormulacionTintaDto>>> GetFormulacion()
        {
            var formulacion = await _context.formulacionTinta.ToListAsync();

            var formulacionDto = _mapper.Map<List<FormulacionTintaDto>>(formulacion);

            return Ok(formulacionDto);
        }

        // GET api/<formulacionTintaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FormulacionTintaDto>> GetFormulacion(int id)
        {
            var formulacion = await _context.formulacionTinta.FirstOrDefaultAsync(f => f.idFormulacion == id);

            if (formulacion == null)
            {
                return NotFound();
            }
            var formulacionDto = _mapper.Map<FormulacionTintaDto>(formulacion);

            return Ok(formulacionDto);

        }

        // POST api/<formulacionTintaController>
        [HttpPost("post")]
        public async Task<ActionResult<AddFormulacionTintaDto>> PostFormulacion(AddFormulacionTintaDto addFormulacionTintaDto)
        {
            try
            {
                var formulacion = _mapper.Map<formulacionTinta>(addFormulacionTintaDto);

                _context.formulacionTinta.Add(formulacion);
                await _context.SaveChangesAsync();

                // Mapeamos de vuelta al DTO para la respuesta
                var respuestaDto = _mapper.Map<AddFormulacionTintaDto>(formulacion);

                // Retornamos el DTO, no la entidad de EF Core
                return CreatedAtAction(nameof(GetFormulacion), new { id = formulacion.idFormulacion }, respuestaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al guardar: " + ex.Message);
            }
        }

        // POST api/<formulacionTintaController>/post/batch
        [HttpPost("post/Batch")]
        public async Task<ActionResult<IEnumerable<AddFormulacionTintaDto>>> PostFormulacionBatch(List<AddFormulacionTintaDto> addDtos)
        {
            try
            {
                // 1. Mapeamos la lista de DTOs a una lista de entidades
                var formulaciones = _mapper.Map<List<formulacionTinta>>(addDtos);

                // 2. AddRange rastrea todos los padres y todos sus hijos automáticamente
                _context.formulacionTinta.AddRange(formulaciones);

                // 3. Un solo SaveChangesAsync inserta todo
                await _context.SaveChangesAsync();

                // 4. Mapeamos de regreso para devolver los IDs generados
                var respuestaDtos = _mapper.Map<List<AddFormulacionTintaDto>>(formulaciones);

                return Ok(respuestaDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al guardar el batch: " + ex.Message);
            }
        }

        // PUT api/<formulacionTintaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFormulacion(int id, UpdateFormulacionTintaDto updateFormulacionTintaDto)
        {
            var formulacion = await _context.formulacionTinta.FindAsync(id);

            if (formulacion == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFormulacionTintaDto, formulacion);
            _context.Entry(formulacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormulacionTintaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFormulacionTintaDto);
        }

        // PUT api/<formulacionTintaController>/put/batch
        [HttpPut("put/batch")]
        public async Task<IActionResult> PutFormulacionBatch(List<UpdateFormulacionTintaDto> updateDtos)
        {
            try
            {
                // 1. Extraemos todos los IDs que queremos actualizar
                var idsToUpdate = updateDtos.Select(dto => dto.idFormulacion).ToList();

                // 2. Traemos de la BD los padres y SUS HIJOS de un solo golpe
                var formulacionesDb = await _context.formulacionTinta
                    .Include(f => f.especificacionTintas)
                    .Where(f => idsToUpdate.Contains(f.idFormulacion))
                    .ToListAsync();

                // 3. Iteramos para actualizar cada registro
                foreach (var dto in updateDtos)
                {
                    var formulacionDb = formulacionesDb.FirstOrDefault(f => f.idFormulacion == dto.idFormulacion);

                    if (formulacionDb != null)
                    {
                        // A) Actualizamos los datos del padre (ignorando la colección de hijos en AutoMapper si es posible)
                        _mapper.Map(dto, formulacionDb);

                        // B) Lógica segura para actualizar los hijos (Detalles)
                        if (dto.especificacionTintas != null)
                        {
                            // - Eliminar hijos que están en la BD pero NO en el DTO
                            var idsHijosDto = dto.especificacionTintas.Select(h => h.idEspecificacion).ToList();
                            var hijosParaEliminar = formulacionDb.especificacionTintas
                                .Where(h => h.idEspecificacion != 0 && !idsHijosDto.Contains(h.idEspecificacion))
                                .ToList();

                            foreach (var hijoEliminar in hijosParaEliminar)
                            {
                                _context.especificacionTintas.Remove(hijoEliminar);
                            }

                            // - Actualizar existentes o agregar nuevos
                            foreach (var hijoDto in dto.especificacionTintas)
                            {
                                if (hijoDto.idEspecificacion == 0) // Es uno nuevo agregado en la edición
                                {
                                    var nuevoHijo = _mapper.Map<especificacionTintas>(hijoDto);
                                    nuevoHijo.idFormulacion = formulacionDb.idFormulacion;
                                    formulacionDb.especificacionTintas.Add(nuevoHijo);
                                }
                                else // Es uno existente, lo actualizamos
                                {
                                    var hijoDb = formulacionDb.especificacionTintas
                                        .FirstOrDefault(h => h.idEspecificacion == hijoDto.idEspecificacion);

                                    if (hijoDb != null)
                                    {
                                        _mapper.Map(hijoDto, hijoDb);
                                    }
                                }
                            }
                        }
                    }
                }

                // 4. Guardamos todos los cambios de todas las formulaciones y sus hijos
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Lote actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el batch: " + ex.Message);
            }
        }

        private bool FormulacionTintaExists(int id)
        {
            return _context.formulacionTinta.Any(e => e.idFormulacion == id);
        }
    }
}
