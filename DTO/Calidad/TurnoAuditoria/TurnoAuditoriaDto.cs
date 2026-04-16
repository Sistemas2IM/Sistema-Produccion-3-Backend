namespace Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria
{
    public class TurnoAuditoriaDto
    {
        public int idTurnoAuditor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? auditor { get; set; }

        public string? nombreAuditor { get; set; }

        public int? turno { get; set; }

        public string? nombreTurno { get; set; }

        public int? estado { get; set; }

        public int? tipoReporte { get; set; }

        public string? aprobadoPor { get; set; }

        public string? nombreAprobador { get; set; }

        public DateTime? fechaAprobacion { get; set; }

        public bool? cancelado { get; set; }

        public bool? archivado { get; set; }
    }
}
