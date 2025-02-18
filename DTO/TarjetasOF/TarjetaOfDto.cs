using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class TarjetaOfDto
    {
        public int oF { get; set; }

        public int? oV { get; set; }

        public int? idEstadoOf { get; set; }

        public string? serieOf { get; set; }

        public string? tipoDeOrden { get; set; }

        public string? nombreOf { get; set; }

        public string? productoOf { get; set; }

        public string? lineaDeNegocio { get; set; }

        public string? clienteOf { get; set; }

        public string? descipcionOf { get; set; }

        public string? vendedorOf { get; set; }

        public decimal? cantidadOf { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaVencimiento { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? inicio { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? finalizacion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaCreacion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaUltimaActualizacion { get; set; }

        public int? posicion { get; set; }

        public bool? archivada { get; set; }

        public decimal? porcentajeCompletado { get; set; }

        public int? procesosCompletados { get; set; }

        public int? procesosPendientes { get; set; }

        public int? totalProcesos { get; set; }

        public bool? cerrada { get; set; }

        public string? codArticulo { get; set; }

        public List<EtiquetaOfDto>? etiquetaDto { get; set; }

        public string? estadonombre { get; set; }

        //public EstadoOfDto? estadoOfDto { get; set; }

    }
}
