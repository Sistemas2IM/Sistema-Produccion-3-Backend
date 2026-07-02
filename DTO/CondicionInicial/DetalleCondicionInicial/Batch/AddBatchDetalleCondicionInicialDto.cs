namespace Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial.Batch
{
    public class AddBatchDetalleCondicionInicialDto
    {
        public int idCondicionInicial { get; set; }

        public int idVariable { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? valor { get; set; }

        public string? valorObjetivo { get; set; }

        public string? valorMaximo { get; set; }

        public string? valorMinimo { get; set; }

        public int? idUnidad { get; set; }

        public string? tolerancia { get; set; }

        public bool? dentroDeRango { get; set; }

        public string? observaciones { get; set; }
    }
}
