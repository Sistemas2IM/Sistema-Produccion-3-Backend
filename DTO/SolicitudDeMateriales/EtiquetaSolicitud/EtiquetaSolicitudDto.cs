namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud
{
    public class EtiquetaSolicitudDto
    {
        public int idEtiqSolicitud { get; set; }

        public int? idSolicitud { get; set; }

        public int? idEtiqueta { get; set; }

        public string? color { get; set; }

        public string? texto { get; set; }
    }
}
