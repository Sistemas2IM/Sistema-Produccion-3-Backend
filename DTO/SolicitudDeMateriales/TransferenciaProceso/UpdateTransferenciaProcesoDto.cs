namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso
{
    public class UpdateTransferenciaProcesoDto
    {
        public int? idLote { get; set; }

        public string? unidadDeMedida { get; set; }

        public string? tipo { get; set; }

        public string? estado { get; set; }

        public int? idOrigen { get; set; }

        public DateTime? fechaEnvio { get; set; }

        public string? enviadoPor { get; set; }

        public int? cantidadEnviada { get; set; }

        public int? idDestino { get; set; }

        public DateTime? fechaRecepcion { get; set; }

        public string? recibidoPor { get; set; }

        public int? cantidadConfirmada { get; set; }

        public string? observaciones { get; set; }

        public int? idProduccion { get; set; }

        public int? areaOrigen { get; set; }

        public int? areaDestino { get; set; }

        public int? oFDestino { get; set; }

        public int? idSolicitudOrigen { get; set; }

        public string? idMaterialSAP { get; set; }

        public string? materialDescripcion { get; set; }
    }
}
