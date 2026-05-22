namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina.ValeBobinaCorteEstado
{
    public class ValeBobinaCorteEstadoDto
    {
        public int idCorteEstado { get; set; }

        public int? idVale { get; set; }

        public int? idDetalleReporte { get; set; }

        public bool? procesadoEnSAP { get; set; }

        public string? procesadoPor { get; set; }

        public DateTime? fechaProcesado { get; set; }
    }
}
