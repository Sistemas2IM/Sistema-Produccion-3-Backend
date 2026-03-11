namespace Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso.Batch
{
    public class UpdateBatchDetalleAuditoriaProceso
    {
        public int idDetalle { get; set; }

        public int? idAuditoria { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
