namespace Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor
{
    public class UpdateSecuenciaColorDto
    {
        public int? idFichaProceso { get; set; }

        public int? unidadImpresion { get; set; }

        public string? caraImpresion { get; set; }

        public string? color { get; set; }

        public decimal? presion { get; set; }

        public string? densidadSTD { get; set; }

        public string? porcentajeAgua { get; set; }
    }
}
