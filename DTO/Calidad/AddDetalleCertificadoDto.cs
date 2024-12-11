namespace Sistema_Produccion_3_Backend.DTO.Calidad
{
    public class AddDetalleCertificadoDto
    {
        public int? idCertificado { get; set; }

        public int? idCaracterista { get; set; }

        public int? numeroFila { get; set; }

        public string? especificacion { get; set; }

        public string? resultado { get; set; }
    }
}
