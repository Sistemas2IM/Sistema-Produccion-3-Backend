using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class EstadoReporteDto
    {
        public int idEstadoReporte { get; set; }

        public string? nombreEstado { get; set; }

        public int? tipoReporte { get; set; }
    }
}
