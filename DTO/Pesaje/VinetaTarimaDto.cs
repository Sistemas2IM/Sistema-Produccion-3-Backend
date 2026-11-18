namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    public class VinetaTarimaDto
    {
        public string codigoVineta { get; set; } = null!;

        public string? codLote { get; set; }

        public string? of { get; set; }

        public string? cliente { get; set; }

        public string? material { get; set; }

        public decimal pesoNeto { get; set; }

        public int? conteoEstimado { get; set; }

        public bool manual { get; set; }

        public DateTime fechaPesaje { get; set; }

        public string pesadoPor { get; set; } = null!;

        public int reimpresiones { get; set; }
    }
}
