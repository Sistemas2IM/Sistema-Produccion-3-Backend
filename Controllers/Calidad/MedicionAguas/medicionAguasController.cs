using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.MedicionAguas;
using Sistema_Produccion_3_Backend.DTO.Calidad.MedicionAguas.Batch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.MedicionAguas
{
    [Route("api/[controller]")]
    [ApiController]
    public class medicionAguasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public medicionAguasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<medicionAguasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MedicionAguasDto>>> GetMedicionAguas()
        {
            var medicionAguas = await _context.medicionAguas.ToListAsync();

            var medicionAguasDto = _mapper.Map<List<MedicionAguasDto>>(medicionAguas);

            return Ok(medicionAguasDto);
        }

        // GET api/<medicionAguasController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MedicionAguasDto>> GetMedicionAguas(int id)
        {
            var medicionAguas = await _context.medicionAguas.FirstOrDefaultAsync(m => m.idMedicionAguas == id);

            if (medicionAguas == null)
            {
                return NotFound();
            }
            var medicionAguasDto = _mapper.Map<MedicionAguasDto>(medicionAguas);

            return Ok(medicionAguasDto);
        }

        // POST api/<medicionAguasController>
        [HttpPost("post")]
        public async Task<ActionResult<AddMedicionAguasDto>> PostMedicionAguas(AddMedicionAguasDto addMedicionAguasDto)
        {
            try
            {
                var medicionAguas = _mapper.Map<medicionAguas>(addMedicionAguasDto);

                _context.medicionAguas.Add(medicionAguas);
                await _context.SaveChangesAsync();

                var medicionAguasDto = _mapper.Map<MedicionAguasDto>(medicionAguas);

                return Ok(medicionAguasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<medicionAguasController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutMedicionAgua(int id, UpdateMedicionAguasDto updateMedicion)
        {
            var meidicionAguas = await _context.medicionAguas.FindAsync(id);

            if (meidicionAguas == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMedicion, meidicionAguas);
            _context.Entry(meidicionAguas).State = EntityState.Modified;

            try 
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicionAguasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateMedicion);
        }

        // POST BATCH
        [HttpPost("post/Batch")]
        public async Task<IActionResult> BatchPostMedicion([FromBody] BatchAddMedicionAguasDto batchAddDto)
        {
            if (batchAddDto.addBatchMedicionAguas == null || !batchAddDto.addBatchMedicionAguas.Any())
            {
                return BadRequest("La lista de mediciones de aguas está vacía.");
            }

            var medicionAguas = batchAddDto.addBatchMedicionAguas.Select(dto => _mapper.Map<medicionAguas>(dto)).ToList();
            await _context.medicionAguas.AddRangeAsync(medicionAguas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new
            {
                Message = $"{medicionAguas.Count} mediciones de aguas agregadas exitosamente.",
                MedicionesAgregadas = medicionAguas
            });
        }

        // PUT BATCH
        [HttpPut("put/Batch")]
        public async Task<IActionResult> BatchPutMedicion([FromBody] BatchUpdateMedicionAguasDto batchUpdateDto)
        {
            if (batchUpdateDto.updateBatchMedicionAguas == null || !batchUpdateDto.updateBatchMedicionAguas.Any())
            {
                return BadRequest("La lista de mediciones de aguas para actualizar está vacía.");
            }
            var medicionAguasToUpdate = new List<medicionAguas>();
            foreach (var updateDto in batchUpdateDto.updateBatchMedicionAguas)
            {
                var existingMedicion = await _context.medicionAguas.FindAsync(updateDto.idMedicionAguas);
                if (existingMedicion == null)
                {
                    return NotFound($"No se encontró la medición de aguas con ID {updateDto.idMedicionAguas}.");
                }
                _mapper.Map(updateDto, existingMedicion);
                medicionAguasToUpdate.Add(existingMedicion);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new
            {
                Message = $"{medicionAguasToUpdate.Count} mediciones de aguas actualizadas exitosamente.",
                MedicionesActualizadas = medicionAguasToUpdate
            });
        }

        private bool MedicionAguasExists(int id)
        {
            return _context.medicionAguas.Any(e => e.idMedicionAguas == id);
        }
    }
}
