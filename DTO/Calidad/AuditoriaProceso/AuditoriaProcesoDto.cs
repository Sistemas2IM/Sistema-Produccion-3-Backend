using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso
{
    public class AuditoriaProcesoDto
    {
        public int idAuditoria { get; set; }

        public int? oF { get; set; }

        public int? idProceso { get; set; }

        public int? maquina { get; set; }

        public string? auditor { get; set; }

        public string? operador { get; set; }

        public string? supervisor { get; set; }

        public string? tipoImpresion { get; set; }

        public DateTime? fechaAuditoria { get; set; }

        public int? estado { get; set; }

        public int? tipoProceso { get; set; }

        public string? observaciones { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? creadoPor { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }

        public int? tipoReporte { get; set; }

        public int? turnoAuditoria { get; set; }

        public bool? completo { get; set; }

        public List<DetalleAuditoriaProcesoDto>? detalleAuditoriaProceso { get; set; }
    }
}
