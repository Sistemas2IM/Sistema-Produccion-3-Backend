using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion
{
    public class LogProgramacionDto
    {
        public int? idLogProgramacion { get; set; }

        public int? tablero { get; set; }

        public string? nombreTablero { get; set; }

        public string? programadoPor { get; set; }

        public string? nombreProgramadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? comentario { get; set; }

        public List<LogProgramacionDetalleDto>? logProgramacionDetalle { get; set; } = null;
    }
}
