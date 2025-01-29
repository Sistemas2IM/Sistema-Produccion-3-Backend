namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class UpdateBatchReporteOperador
    {
        public string? idReporte { get; set; }

        public int? idEstadoReporte { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }
    }
}
