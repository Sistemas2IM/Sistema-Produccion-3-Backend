namespace Sistema_Produccion_3_Backend.DTO.ValidacionArranque.DetalleValidacionArranque
{
    public class UpdateDetalleValidacionArranqueDto
    {
        public int idValidacionArranque { get; set; }

        public string? colores { get; set; }

        public decimal? densidad { get; set; }

        public decimal? porcentajeAgua { get; set; }
    }
}
