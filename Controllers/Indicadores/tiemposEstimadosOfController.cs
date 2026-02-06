using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models; // Namespace de los Result Models

namespace Sistema_Produccion_3_Backend.Controllers.Indicadores
{
    [Route("api/[controller]")]
    [ApiController]
    public class tiemposEstimadosOfController : ControllerBase
    {
        // Reemplaza 'TuDbContext' por el nombre real de tu clase de contexto
        private readonly base_nuevaContextProcedures _contextSP;

        public tiemposEstimadosOfController(base_nuevaContextProcedures contextSP)
        {
            _contextSP = contextSP;
        }

        /// <summary>
        /// Obtiene el detalle de procesos y fechas estimadas de una OF específica
        /// </summary>
        [HttpGet("detalle/{idOF}")]
        public async Task<ActionResult<IEnumerable<FFE_DetalleProcesosResult>>> GetDetalle(int idOF)
        {
            try
            {
                var result = await _contextSP.FFE_DetalleProcesosAsync(idOF);

                if (result == null || result.Count == 0)
                    return NotFound($"No se encontraron procesos para la OF {idOF}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el resumen ejecutivo y semáforo de cumplimiento de una OF
        /// </summary>
        [HttpGet("resumen/{idOF}")]
        public async Task<ActionResult<FFE_ResumenOFResult>> GetResumen(int idOF)
        {
            try
            {
                var result = await _contextSP.FFE_ResumenOFAsync(idOF);

                if (result == null || result.Count == 0)
                    return NotFound($"No se pudo generar el resumen para la OF {idOF}");

                // Como es un resumen único, devolvemos el primer registro
                return Ok(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
