using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.Asignacion;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.CamposPersonalizados;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.DetalleProceso;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class ListaProcesoOfDto
    {
        public int idProceso { get; set; }

        public int? oF { get; set; }

        public int? oV { get; set; }

        public string? nombreTarjeta { get; set; }

        public string? cliente { get; set; }

        public string? codProd { get; set; }

        public string? vendedor { get; set; }

        public string? productoOf { get; set; }

        public int? secuencia { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaFinalizacion { get; set; }

        public decimal? tiempoEstimado { get; set; }

        public decimal? horasTotales { get; set; }

        public int? posicion { get; set; }

        public string? programadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? tipoObjeto { get; set; }

        public bool? archivada { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        public string? lineaNegocio { get; set; }

        public string? cantRequerida { get; set; }

        public string? tipoOrden { get; set; }

        public string? unidadMedida { get; set; }

        public string? fsc { get; set; }

        public string? tipoMaquinaSAP { get; set; }

        public string? idMaquinaSAP { get; set; }

        public object? DetalleProceso { get; set; }

        public string? serie { get; set; }

        public string? serieNumeracion { get; set; }

        public string? tiroRetiro { get; set; }

        public DateTime? fechaActualización { get; set; }

        public string? comentario { get; set; }

        public string? actualizadoPor { get; set; }

        public bool? muestra { get; set; }

        public bool? cancelada { get; set; }

        public string? indicador { get; set; }

        public bool? corridaCombinada { get; set; }

        public int? lineNumSAP { get; set; }

        public int? secuenciaArea { get; set; }

        public string? indicadorProces { get; set; }

        public bool? reproceso { get; set; }

        public string? correlativoCC { get; set; }

        public List<AsignacionDto>? asignacionDto { get; set; }

        public ProcesoPosturasOfDto? PosturasOfDto { get; set; }

        public ProcesoTablerosOfDto? TablerosOfDto { get; set; }

        public List<TarjetaCampoDto>? tarjetaCampoDto { get; set; }

        public List<TarjetaEtiquetaDto>? tarjetaEtiquetaDto { get; set; }

        public List<CorridaCombinadaDto>? subordinadas { get; set; }

    }
}
