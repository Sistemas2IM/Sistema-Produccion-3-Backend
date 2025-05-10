namespace Sistema_Produccion_3_Backend.DTO.CorridaCombinada
{
    public class CorridaCombinadaDto
    {
        public int idRelacion { get; set; }

        public int? maestro { get; set; }

        public int? subordinado { get; set; }

        public string? oF { get; set; } // de subordinado / proceso of

        public string? productoOf { get; set; } // de subordinado / proceso of

        public DateTime? fechaVencimiento { get; set; }

        public string? cantRequerida { get; set; } // de maestro / proceso of

        public string? nombreCliente { get; set; } // de maestro / proceso of
    }
}
