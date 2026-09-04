namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar
{
    public class UpdateConfirmacionPreliminarDto
    {
        public int? oF { get; set; }

        public int? idProceso { get; set; }

        public decimal? cantidadRecibida { get; set; }

        public int? idUnidad { get; set; }

        public int? idTurno { get; set; }

        public string? observaciones { get; set; }

        public int? idEstado { get; set; }

        public int? tipoReporte { get; set; }

        public string? entregadoPor { get; set; }

        public string? operador { get; set; }

        public string? registradoPor { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public bool? cancelado { get; set; }

        public bool? archivado { get; set; }
    }
}
