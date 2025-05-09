using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas
{
    public class UpProcesoOfMaquinas
    {
        public int? oV { get; set; }

        public string? idMaterial { get; set; }

        public string? nombreTarjeta { get; set; }

        public string? productoOf { get; set; }

        public string? descripcionOf { get; set; }

        public int? secuencia { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }

        public int? pliegosRecibidos { get; set; }

        public int? pliegosEntregados { get; set; }

        public int? pliegosDanados { get; set; }

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

        public string? idMaquinaSAP { get; set; }

        public string? tipoMaquinaSAP { get; set; }

        public object? DetalleProceso { get; set; }

        public string? serie { get; set; }

        public string? serieNumeracion { get; set; }

        public string? tiroRetiro { get; set; }

        public DateTime? fechaActualización { get; set; }

        public string? comentario { get; set; }

        public string? actualizadoPor { get; set; }

        public bool? muestra { get; set; }

        public string? indicador { get; set; }

        public bool? corridaCombinada { get; set; }
    }
}
