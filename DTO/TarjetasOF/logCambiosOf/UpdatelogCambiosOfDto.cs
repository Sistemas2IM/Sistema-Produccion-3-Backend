namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF.logCambiosOf
{
    public class UpdatelogCambiosOfDto
    {
        public int log_id { get; set; }

        public int? oF { get; set; }

        public string? usuario_id { get; set; }

        public DateTime? fecha_hora { get; set; }

        public string? estado_anterior { get; set; }

        public string? nuevo_estado { get; set; }
    }
}
