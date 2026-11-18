namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    public class AddPesajeTarimaDto
    {
        public string? tipoUnidad { get; set; }

        public decimal pesoBruto { get; set; }

        public int? idTipoTara { get; set; }

        public decimal tara { get; set; }

        public decimal pesoNeto { get; set; }

        public decimal? gramaje { get; set; }

        public int? conteoEstimado { get; set; }

        public bool manual { get; set; }

        public string? idBascula { get; set; }
    }
}
