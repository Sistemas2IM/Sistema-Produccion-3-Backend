namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego
{
    public class lotePliegoDto
    {
        public int idLote { get; set; }

        public int? idSolicitud { get; set; }

        public decimal? largo { get; set; }

        public decimal? ancho { get; set; }

        public int? cantidadPliegos { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? creadoPor { get; set; }

        public string? idMaterialSAP { get; set; }

        public string? codLote { get; set; }

        public string? estado { get; set; }

        public int? procesoOrigen { get; set; }

        public decimal? peso { get; set; }

        public string? codigoBobinaSAP { get; set; }

        public bool? tira { get; set; }

        public int? cantidadPendiente { get; set; }

        public int? unidadMedida { get; set; }

        public string? nombreUnidad { get; set; }

        public string? simboloUnidad { get; set; }
    }
}
