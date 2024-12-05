namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas.GeneralidadColor
{
    public class AddGeneralidadColorDto
    {
        public int? idEspacioColor { get; set; }

        public string? descripcionTinta { get; set; }

        public string? proveedorTinta { get; set; }

        public string? porcentajeTinta { get; set; }
    }
}
