namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion
{
    public class UpdateLogProgramacionDto
    {
        public int? tablero { get; set; }

        public string? programadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? comentario { get; set; }
    }
}
