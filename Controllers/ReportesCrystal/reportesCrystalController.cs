/*using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Reportes_Cystal_Nexo.Controllers.ReportesCrystal
{
    [Route("api/[controller]")]
    [ApiController]
    public class reportesCrystalController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<reportesCrystalController> _logger;

        public reportesCrystalController(IWebHostEnvironment env, ILogger<reportesCrystalController> logger)
        {
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Genera un reporte en formato PDF
        /// </summary>
        /// <param name="idReporte">Nombre del archivo del reporte (sin extensión .rpt)</param>
        /// <param name="parametros">Diccionario de parámetros para el reporte</param>
        /// <returns>Archivo PDF con el reporte generado</returns>
        [HttpGet("generar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GenerarReporte(
            [Required] string idReporte,
            [FromQuery] Dictionary<string, string> parametros = null)
        {
            _logger.LogInformation($"Solicitud para generar reporte: {idReporte}");

            try
            {
                string reportPath = Path.Combine(_env.ContentRootPath, "Reportes", $"{idReporte}.rpt");

                if (!System.IO.File.Exists(reportPath))
                {
                    _logger.LogWarning($"Reporte no encontrado: {idReporte}");
                    return NotFound($"El reporte {idReporte} no fue encontrado.");
                }

                var reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                if (parametros != null)
                {
                    foreach (var param in parametros)
                    {
                        reportDocument.SetParameterValue(param.Key, param.Value);
                    }
                }

                var stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf", $"{idReporte}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar el reporte {idReporte}");
                return StatusCode(500, $"Error al generar el reporte: {ex.Message}");
            }
        }

        /// <summary>
        /// Genera un reporte en formato PDF (método POST)
        /// </summary>
        [HttpPost("generar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GenerarReportePost([FromBody] ReporteRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdReporte))
            {
                return BadRequest("La solicitud debe incluir un IdReporte válido");
            }

            return GenerarReporte(request.IdReporte, request.Parametros);
        }
    }
    /// <summary>
    /// Modelo de solicitud para generación de reportes
    /// </summary>
    public class ReporteRequest
    {
        /// <summary>
        /// Nombre del archivo del reporte (sin extensión .rpt)
        /// </summary>
        [Required]
        public string IdReporte { get; set; }

        /// <summary>
        /// Parámetros para el reporte
        /// </summary>
        public Dictionary<string, string> Parametros { get; set; }
    }
}*/
