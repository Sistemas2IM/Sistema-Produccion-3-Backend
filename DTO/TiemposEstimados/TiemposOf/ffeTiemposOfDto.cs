namespace Sistema_Produccion_3_Backend.DTO.TiemposEstimados.TiemposOf
{
    public class ffeTiemposOfDto
    {
        public int idOF { get; set; }

        public DateTime? Inicio_Real { get; set; }

        public DateTime? Inicio_Estimado { get; set; }

        public DateTime? Fin_Real { get; set; }

        public DateTime? Fin_Proyectado { get; set; }

        public DateTime? Vencimiento_SAP { get; set; }

        public int? Atrasada { get; set; }

        public string? Estado_Causa { get; set; }

        public DateTime? Actualizado { get; set; }
    }
}
