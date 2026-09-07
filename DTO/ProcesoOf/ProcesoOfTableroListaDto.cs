using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.Asignacion;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.CamposPersonalizados;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class ProcesoOfTableroListaDto
    {
        public int idProceso { get; set; }

        public int? oF { get; set; }

        public int? posicion { get; set; }

        public int? idTablero { get; set; }

        public string? nombrePostura { get; set; }

        public int? idPostura { get; set; }

        public string? productoOf { get; set; }

        public string? cliente { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        public DateTime? fechaFinalizacion { get; set; }

        public string? vendedor { get; set; }

        public string? cantRequerida { get; set; }

        public string? codProd { get; set; }

        public string? tipoDeOrden { get; set; } // Nota: Se mantuvo según lista

        public string? lineaNegocio { get; set; }

        public decimal? tiempoEstimado { get; set; }

        public string? comentario { get; set; }

        public List<TarjetaEtiquetaDto>? tarjetaEtiquetaDto { get; set; }

        public string? fsc { get; set; }

        public string? tipoOrden { get; set; }

        public string? unidadMedida { get; set; }

        public List<AsignacionDto>? asignacionDto { get; set; }

        public object? DetalleProceso { get; set; }

        public MaterialDto? materialDto { get; set; }

        public string? serie { get; set; }

        public bool? muestra { get; set; }

        public string? tipoMaquinaSAP { get; set; }

        public string? serieNumeracion { get; set; }

        public string? tiroRetiro { get; set; }

        public string? indicador { get; set; }

        public string? indicadorProceso { get; set; }

        public bool? reproceso { get; set; }

        public bool? corridaCombinada { get; set; }

        public bool? bloqueada { get; set; }

        public decimal? tiempoConsumido { get; set; }

        public decimal? tiempoRestante { get; set; }

        public DateTime? fechaVencimientoAnterior { get; set; }

        public DateTime? fechaVencimientoNueva { get; set; }

        public List<CorridaCombinadaDto>? subordinadas { get; set; }

        public string? correlativoCC { get; set; }

        // campo bandera, no existe en DB es para este DTO
        public bool? detOps { get; set; }
    }
}
