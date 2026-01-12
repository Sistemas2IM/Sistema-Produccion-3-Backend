using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas.Batch;
using Sistema_Produccion_3_Backend.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class bobinasAsignadasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly base_nuevaContextProcedures _contextSP;

        public bobinasAsignadasController(base_nuevaContext context, IMapper mapper, base_nuevaContextProcedures contextSP)
        {
            _context = context;
            _mapper = mapper;
            _contextSP = contextSP;
        }

        // GET: api/<bobinasAsignadasController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<BobinasAsignadasDto>>> GetBobinas()
        {
            var bobinasAsignadas = await _context.bobinasAsignadas.ToListAsync();

            var bobinasAsignadasDto = _mapper.Map<List<BobinasAsignadasDto>>(bobinasAsignadas);

            return Ok(bobinasAsignadasDto);
        }

        // GET api/<bobinasAsignadasController>/5
        //[HttpGet("get/idProceso/{id}")]
        //public async Task<ActionResult<IEnumerable<BobinasAsignadasDto>>> GetBobinasId(int id)
        //{
        //    var bobinasAsignadas = await _context.bobinasAsignadas
        //        .FirstOrDefaultAsync(b => b.idProceso == id);

        //    var bobinasAsignadasDto = _mapper.Map<BobinasAsignadasDto>(bobinasAsignadas);

        //    if (bobinasAsignadasDto == null)
        //    {
        //        return NotFound($"No se encontro el registro con el id: {id}");
        //    }

        //    return Ok(bobinasAsignadasDto);
        //}

        // GET: api/<bobinasAsignadasController>
        [HttpGet("get/idProceso/{id}")]
        public async Task<ActionResult<IEnumerable<BobinasAsignadasDto>>> GetBobinasId(int id)
        {
            var bobinasAsignadas = await _context.bobinasAsignadas
                .Where(b => b.idProceso == id)
                .ToListAsync();

            var bobinasAsignadasDto = _mapper.Map<List<BobinasAsignadasDto>>(bobinasAsignadas);

            return Ok(bobinasAsignadasDto);
        }

        [HttpGet("InfoConvercionBobina/{codBobina}")]
        public async Task<ActionResult<List<infoConvercionBobinaResult>>> GetInfoConvercionBobina(string codBobina)
        {
            // Validación básica para evitar llamadas vacías
            if (string.IsNullOrWhiteSpace(codBobina))
            {
                return BadRequest("El código de la bobina no puede estar vacío.");
            }

            try
            {
                // 1. Llamar al servicio que ejecuta el SP
                var resultados = await _contextSP.infoConvercionBobinaAsync(codBobina);

                // 2. Validar si retornó datos
                if (resultados == null || resultados.Count == 0)
                {
                    return NotFound($"No se encontraron datos para la bobina con código: {codBobina}");
                }

                // 3. Retornar los datos con código 200 OK
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                // Manejo de error genérico (opcional, pero recomendado)
                return StatusCode(500, "Ocurrió un error interno al procesar la bobina: " + ex.Message);
            }
        }

        // POST api/<bobinasAsignadasController>
        [HttpPost("post")]
        public async Task<ActionResult<BobinasAsignadasDto>> PostBobina(AddBobinasAsignadasDto bobinasAsignadasDto)
        {
            var bobinaAsignada = _mapper.Map<bobinasAsignadas>(bobinasAsignadasDto);
            _context.bobinasAsignadas.Add(bobinaAsignada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBobinas", new { id = bobinaAsignada.idProceso }, bobinaAsignada);
        }

        // POST BATCH
        [HttpPost("post/batch")]
        public async Task<IActionResult> BatchAddBobinas([FromBody] BatchAddBobinasAsignadasDto batchAddDto)
        {
            if (batchAddDto.addBatchBobinasAsignadas == null || !batchAddDto.addBatchBobinasAsignadas.Any())
            {
                return BadRequest();
            }

            var bobinasAsignadas = batchAddDto.addBatchBobinasAsignadas.Select(dto => _mapper.Map<bobinasAsignadas>(dto))
                .ToList();

            await _context.bobinasAsignadas.AddRangeAsync(bobinasAsignadas);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            return Ok(new
            {
                Message = "Bobinas agregadas exitosamente.",
                BobinasAsignadas = bobinasAsignadas
            });
        }

        // DELTE BATCH
        [HttpDelete("delete/batch")]
        // Cambiamos List<int> por List<BobinaDto>
        public async Task<IActionResult> DeleteBobinas([FromBody] List<BobinaObjectDto> bobinasABorrar)
        {
            if (bobinasABorrar == null || !bobinasABorrar.Any())
            {
                return BadRequest("La lista no puede estar vacía.");
            }

            // Paso 1: Obtener los IDs de proceso para hacer una primera búsqueda rápida en BD
            var procesosIds = bobinasABorrar.Select(x => x.IdProceso).Distinct().ToList();

            // Paso 2: Traer de la base de datos los "candidatos"
            // Buscamos todas las bobinas que tengan esos idProceso.
            // (EF Core no suele traducir bien comparaciones de objetos complejos locales vs DB,
            // por eso filtramos primero por el ID numérico y luego refinamos en memoria).
            var candidatos = await _context.bobinasAsignadas
                .Where(b => procesosIds.Contains(b.idProceso.Value)) // Asumo que en BD es nullable por tu error anterior
                .ToListAsync();

            // Paso 3: Filtrar en memoria los pares exactos
            // Comparamos lo que vino de la DB con lo que mandó el usuario
            var registrosParaEliminar = candidatos
                .Where(dbRecord => bobinasABorrar.Any(input =>
                    input.IdProceso == dbRecord.idProceso &&
                    input.CodigoBobina == dbRecord.codigoBobina))
                .ToList();

            if (!registrosParaEliminar.Any())
            {
                return NotFound("No se encontraron coincidencias exactas para eliminar.");
            }

            // Paso 4: Eliminar
            _context.bobinasAsignadas.RemoveRange(registrosParaEliminar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar: {ex.Message}");
            }

            return Ok($"Se eliminaron {registrosParaEliminar.Count} registros correctamente.");
        }

    }
}
