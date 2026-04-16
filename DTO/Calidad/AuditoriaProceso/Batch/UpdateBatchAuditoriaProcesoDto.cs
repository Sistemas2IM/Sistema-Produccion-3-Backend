namespace Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.Batch
{
    public class UpdateBatchAuditoriaProcesoDto
    {
        public int idAuditoria { get; set; }

        public int? estado { get; set; }

        public string? actualizadoPor { get; set; }

        public DateTime? fechaActualizacion { get; set; }
    }
}
