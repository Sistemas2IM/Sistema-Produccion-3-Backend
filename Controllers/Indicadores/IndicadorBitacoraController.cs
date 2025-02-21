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

        // GET: api/IndicadoresReporte/5
        [HttpGet("indicadoresBitacora/{idReporte}")]
        public async Task<ActionResult<IndicadoresReporteDto>> GetIndicadoresReporte(string idReporte)
        {
            // Llamar al método generado por EF Core Power Tools
            var resultados = await _contextSP.IndicadoresReporteAsync(idReporte);

            if (resultados == null || resultados.Count == 0)
            {
                return NotFound("No se encontró el reporte con el id: " + idReporte);
            }

            // Devolver el primer resultado (asumiendo que solo hay uno)
            var resultado = resultados.First();

            return Ok(resultado);
        }
    }
}
