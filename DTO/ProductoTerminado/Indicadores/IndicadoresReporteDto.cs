namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado.Indicadores
{
    public class IndicadoresReporteDto
    {
        public string? Tiempo { get; set; } // Formato HH:MM:SS
        public int? VelocidadNominal { get; set; }
        public int? VelocidadEfectiva { get; set; }
    }
}
