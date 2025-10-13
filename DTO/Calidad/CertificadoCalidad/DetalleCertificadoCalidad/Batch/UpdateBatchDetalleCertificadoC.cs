namespace Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad.Batch
{
    public class UpdateBatchDetalleCertificadoC
    {
        public int? idDetalle { get; set; }

        public int? idCertificadoCalidad { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public string? resultado { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
