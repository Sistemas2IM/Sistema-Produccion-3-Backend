using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.Indicadores;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Indicadores
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicadorBitacoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly base_nuevaContextProcedures _contextSP;

        public IndicadorBitacoraController(base_nuevaContext context, IMapper mapper, base_nuevaContextProcedures contextSP)
        {
            _context = context;
            _mapper = mapper;
            _contextSP = contextSP;
        }

        [HttpGet("indicadoresBitacora/{idReporte}")]
        public async Task<ActionResult<IndicadoresReporteDto>> GetIndicadoresReporte(string idReporte)
        {
            // Obtener los resultados del endpoint
            var resultados = await _contextSP.IndicadoresReporteAsync(idReporte);

            if (resultados == null || resultados.Count == 0)
            {
                return NotFound("No se encontró el reporte con el id: " + idReporte);
            }

            // Obtener el primer resultado
            var resultado = resultados.First();

            // Buscar la entidad en la base de datos
            var reporte = await _context.reportesDeOperadores.FindAsync(idReporte);

            if (reporte == null)
            {
                return NotFound("No se encontró el reporte con el id: " + idReporte);
            }

            // Parsear el string a TimeOnly
            if (TimeOnly.TryParse(resultado.tiempo, out var tiempoOnly))
            {
                // Asignar el TimeOnly al campo de la entidad
                reporte.tiempoTotal = tiempoOnly;
            }
            else
            {
                return BadRequest("El formato del tiempo no es válido.");
            }

            // Actualizar los otros campos
            reporte.velocidadNominal = resultado.velocidad_nominal;
            reporte.velocidadEfectiva = resultado.velocidad_efectiva;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(resultado);
        }

        [HttpGet("TimeInList/{idProceso}")]
        public async Task<ActionResult<List<TimeInListResult>>> GetTimeInList(int idProceso)
        {
            // Obtener los resultados del procedimiento almacenado
            var resultados = await _contextSP.TimeInListAsync(idProceso);

            if (resultados == null || resultados.Count == 0)
            {
                return NotFound("No se encontraron datos para el proceso con el id: " + idProceso);
            }

            // Aquí puedes agregar lógica adicional si necesitas procesar los resultados
            // Por ejemplo, mapear los resultados a un DTO o realizar alguna validación

            return Ok(resultados);
        }

        [HttpGet("resumenOf/{numOf}")]  // ":int" restringe a solo valores enteros
        [ProducesResponseType(typeof(List<ResumenOfResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResumenOf(
        [FromRoute] int numOf,  // Ahora es obligatorio (no nullable)
        CancellationToken cancellationToken)
        {
            try
            {
                var result = await _contextSP.ResumenOfAsync(numOf, cancellationToken: cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
