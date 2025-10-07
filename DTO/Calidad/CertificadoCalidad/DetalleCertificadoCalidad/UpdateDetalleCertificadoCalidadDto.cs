namespace Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad
{
    public class UpdateDetalleCertificadoCalidadDto
    {
        public int? idCertificadoCalidad { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public string? resultado { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
