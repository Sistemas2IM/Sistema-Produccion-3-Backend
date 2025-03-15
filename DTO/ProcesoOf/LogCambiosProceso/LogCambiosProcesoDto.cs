namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogCambiosProceso
{
    public class LogCambiosProcesoDto
    {
        public int log_id { get; set; }

        public int? proceso_id { get; set; }

        public string? usuario_id { get; set; }

        public DateTime? fecha_hora { get; set; }

        public string? estado_anterior { get; set; }

        public string? nuevo_estado { get; set; }
    }
}
