using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.ContenidoEntrega;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.DetalleEntrega;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.EstadoReporte;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Services;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado
{
    public class ProductoTerminadoDto
    {
        public int idEntregaPt { get; set; }

        public int? idMaquina { get; set; }

        public int? idEstadoReporte { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaEntrega { get; set; }

        public string? areaEntrega { get; set; }

        public string? areaRecibe { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaRecepcion { get; set; }

        public int? numeroDeTarimas { get; set; }

        public int? numeroDeCorrugados { get; set; }

        public string? creadoPor { get; set; }

        public string? recibidoPor { get; set; }

        public bool? entregaParcial { get; set; }

        public bool? reportaSobrantes { get; set; }

        public int? cantidadSobrante { get; set; }

        public int? cantidadEntregadaTotal { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? actualizadoPor { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public int? tipoObjeto { get; set; }

        public string? entregadoPor { get; set; }

        public int? of { get; set; }

        public List<ContenidoEntregaDto>? contenidoEntregado { get; set; }

        public List<DetalleEntregaDto>? detalleEntrega { get; set; }

        public EstadoReporteDto? estadoReporteDto { get; set; }

        public MaquinaDto? maquinaDto { get; set; }

        public TarjetaOfDto? tarjetaOfDto { get; set; }
    }
}
