namespace Sistema_Produccion_3_Backend.DTO.Calidad
{
    public class UpdateDetalleCertificadoDto
    {
        public int? idCaracterista { get; set; }

        public int? numeroFila { get; set; }

        public string? especificacion { get; set; }

        public string? resultado { get; set; }
    }
}
