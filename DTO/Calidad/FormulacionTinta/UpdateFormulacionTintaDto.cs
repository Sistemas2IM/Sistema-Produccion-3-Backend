namespace Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta
{
    public class UpdateFormulacionTintaDto
    {
        public int? idFichaProceso { get; set; }

        public string? color { get; set; }

        public string? referencia { get; set; }

        public decimal? cie_L { get; set; }

        public decimal? cie_a { get; set; }

        public decimal? cie_b { get; set; }
    }
}
