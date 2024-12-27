using Sistema_Produccion_3_Backend.DTO.TarjetasOF.EtiquetaOf;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF.Reportes
{
    public class ReportePMTarjetaOf
    {
        public int diasEnPlanta {  get; set; }

        public string? clienteOf { get; set; }

        public string? vendedorOf { get; set; }

        public int oF { get; set; }      

        public string? nombreOf { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaCreacion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaVencimiento { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? inicio { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? finalizacion { get; set; }       

        public decimal? porcentajeCompletado { get; set; }

        public int? procesosCompletados { get; set; }

        public int? procesosPendientes { get; set; }

        public int? totalProcesos { get; set; }      

        public int diasALaEntrega { get; set; }
    }
}
