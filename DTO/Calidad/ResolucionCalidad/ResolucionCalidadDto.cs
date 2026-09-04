namespace Sistema_Produccion_3_Backend.DTO.Calidad.ResolucionCalidad
{
    public class ResolucionCalidadDto
    {
        public int idResolucion { get; set; }

        public string? codigo { get; set; }

        public string? nombre { get; set; }

        public bool implicaCosto { get; set; }

        public bool activo { get; set; }
    }
}
