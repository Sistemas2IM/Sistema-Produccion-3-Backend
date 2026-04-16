namespace Sistema_Produccion_3_Backend.DTO.Buscadores.DTOGlobales
{
    public class SB_SolicitudMaterialesOfDto
    {
        public int idSolicitud { get; set; }

        public int oF { get; set; }
        
        public string? materialDescripcion { get; set; }

        public string? clienteOf { get; set; }

        public string? productoOf { get; set; }

        public string? codArticulo { get; set; }
    }
}
