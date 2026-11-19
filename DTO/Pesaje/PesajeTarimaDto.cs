namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    public class PesajeTarimaDto
    {
        public int idPesaje { get; set; }

        public int correlativo { get; set; }

        public string tipoUnidad { get; set; } = null!;

        public decimal pesoBruto { get; set; }

        public int? idTipoTara { get; set; }

        public decimal tara { get; set; }

        public decimal pesoNeto { get; set; }

        public decimal? gramaje { get; set; }

        public int? conteoEstimado { get; set; }

        public string codigoVineta { get; set; } = null!;

        public bool manual { get; set; }

        public string estado { get; set; } = null!;

        public DateTime fechaPesaje { get; set; }

        public string pesadoPor { get; set; } = null!;

        public string? idBascula { get; set; }

        public int reimpresiones { get; set; }
    }
}
