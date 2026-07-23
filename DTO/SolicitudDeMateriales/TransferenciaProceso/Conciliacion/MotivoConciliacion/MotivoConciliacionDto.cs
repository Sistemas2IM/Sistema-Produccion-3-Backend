namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.MotivoConciliacion
{
    public class MotivoConciliacionDto
    {
        public int idMotivo { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? activo { get; set; }
    }
}
