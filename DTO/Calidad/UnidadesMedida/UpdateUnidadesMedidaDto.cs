namespace Sistema_Produccion_3_Backend.DTO.Calidad.UnidadesMedida
{
    public class UpdateUnidadesMedidaDto
    {
        public string? nombre { get; set; }

        public string? simbolo { get; set; }

        public string? tipo { get; set; }

        public bool? activo { get; set; }

        public decimal? factorConversion { get; set; }
    }
}
