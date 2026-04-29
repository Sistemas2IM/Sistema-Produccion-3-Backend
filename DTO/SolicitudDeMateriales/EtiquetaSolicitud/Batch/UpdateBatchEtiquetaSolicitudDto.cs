namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud.Batch
{
    public class UpdateBatchEtiquetaSolicitudDto
    {
        public int idEtiqSolicitud { get; set; }

        public int? idSolicitud { get; set; }

        public int? idEtiqueta { get; set; }
    }
}
