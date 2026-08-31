namespace Sistema_Produccion_3_Backend.DTO.Calidad.SubtipoDefecto
{
    public class UpdateSubtipoDefectoDto
    {
        public int? idCategoria { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? activo { get; set; }

        public string? severidad { get; set; }
    }
}
