namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar
{
    public class ConfirmacionPreliminarListaDTO
    {
        public int? idPreliminar { get; set; }

        public int? oF { get; set; }
        public string? clienteOf { get; set; }
        public string? nombreProducto { get; set; }
        public string? serieOf { get; set; }

        public int? idProceso { get; set; }
        public int? idMaquina { get; set; }
        public string? nombreMaquina { get; set; }

        public decimal? cantidadRecibida { get; set; }

        public int? idUnidad { get; set; }
        public string? nombreUnidad { get; set; }
        public string? simboloUnidad { get; set; }

        public int? idTurno { get; set; }
        public string? nombreTurno { get; set; }

        public string? observaciones { get; set; }

        public int? idEstado { get; set; }
        public string? nombreEstado { get; set; }

        public int? tipoReporte { get; set; }

        public string? entregadoPor { get; set; }
        public string? nombreEntregadoPor { get; set; }

        public string? operador { get; set; }
        public string? nombreOperador { get; set; }

        public string? registradoPor { get; set; }
        public string? nombreRegistradoPor { get; set; }

        public DateTime? fechaRegistro { get; set; }
        public DateTime? fechaActualizacion { get; set; }
        public string? actualizadoPor { get; set; }
        public string? nombreActualizadoPor { get; set; }

        public bool? cancelado { get; set; }
        public bool? archivado { get; set; }

        // Calculados. Sin las colecciones: solo indicadores.
        public int totalTransferencias { get; set; }
        public decimal? saldo { get; set; }
        public bool tieneConciliacion { get; set; }

        // Campos de la transferencia, tienen que ser sumados
        public decimal? cantidadEnviada { get; set; }
        public decimal? cantidadConfirmada { get; set; }
    }
}