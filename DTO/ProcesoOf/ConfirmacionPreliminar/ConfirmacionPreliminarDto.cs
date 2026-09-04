using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar
{
    public class ConfirmacionPreliminarDto
    {
        public int? idPreliminar { get; set; }

        public int? oF { get; set; }
        // relacion con la tabla orden de fabricacion
        public string? clienteOf { get; set; }
        public string? nombreProducto { get; set; }
        public string? serieOf { get; set; }
        //------------------------------------------

        public int? idProceso { get; set; }
        // relacion con la tabla proceso
        public int? idMaquina { get; set; }
        public string? nombreMaquina { get; set; }
        //------------------------------------------

        public decimal? cantidadRecibida { get; set; }

        public int? idUnidad { get; set; }
        // relacion con la tabla unidad de medida
        public string? nombreUnidad { get; set; }
        public string? simboloUnidad { get; set; }
        //------------------------------------------

        public int? idTurno { get; set; }
        // relacion con la tabla turno
        public string? nombreTurno { get; set; }
        //----------------------------------------

        public string? observaciones { get; set; }

        public int? idEstado { get; set; }
        // relacion con la tabla estado
        public string? nombreEstado { get; set; }
        //----------------------------------------

        public int?tipoReporte { get; set; }

        public string? entregadoPor { get; set; }
        // relacion con la tabla usuario
        public string? nombreEntregadoPor { get; set; }
        //----------------------------------------

        public string? operador { get; set; }
        // relacion con la tabla usuario
        public string? nombreOperador { get; set; }
        //----------------------------------------

        public string? registradoPor { get; set; }
        // relacion con la tabla usuario
        public string? nombreRegistradoPor { get; set; }
        //----------------------------------------

        public DateTime? fechaRegistro { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }
        // relacion con la tabla usuario
        public string? nombreActualizadoPor { get; set; }
        //----------------------------------------

        public bool? cancelado { get; set; }

        public bool? archivado { get; set; }

        // campos calculados -----------------------------
        //total de tranferencias vinculadas
        public int? totalTransferencias { get; set; }

        // saldo cantidad recibida - total de transferencias vinculadas
        public decimal? saldo { get; set; }

        public List<transferenciaProcesoDto> transferenciaProcesos { get; set; }

        // info de la conciliacion
        public ConciliacionResumenDTO? conciliacion { get; set; }

    }
}
