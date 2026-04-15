namespace Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria.Batch
{
    public class UpdateBatchTurnoAuditoriaDto
    {
        public int idTurnoAuditor { get; set; }

        public int? estado { get; set; }

        public string? aprobadoPor { get; set; }

        public DateTime? fechaAprobacion { get; set; }
    }
}
