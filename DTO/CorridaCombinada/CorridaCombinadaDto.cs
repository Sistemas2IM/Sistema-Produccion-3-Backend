namespace Sistema_Produccion_3_Backend.DTO.CorridaCombinada
{
    public class CorridaCombinadaDto
    {
        public int idRelacion { get; set; }

        public int? maestro { get; set; }

        public int? subordinado { get; set; }

        public string? oF { get; set; } // de subordinado / proceso of

        public string? productoOf { get; set; } // de subordinado / proceso of
    }
}
