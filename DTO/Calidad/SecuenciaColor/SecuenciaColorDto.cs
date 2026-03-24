namespace Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor
{
    public class SecuenciaColorDto
    {
        public int idSecuenciaColor { get; set; }

        public int? idFichaProceso { get; set; }

        public int? unidadImpresion { get; set; }

        public string? caraImpresion { get; set; }

        public string? color { get; set; }

        public decimal? presion { get; set; }

        public string? densidadSTD { get; set; }

        public string? porcentajeAgua { get; set; }
    }
}
