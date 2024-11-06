using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sistema_Produccion_3_Backend.Services;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class OVDto
    {
        [Column("oV")]
        [StringLength(200)]
        [Unicode(false)]
        public int? oV1 { get; set; }

        public string? codigoCliente { get; set; }

        public string? cliente { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaOv { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaEntrega { get; set; }

        public string? comentario { get; set; }

        public string? vendedor { get; set; }

        public string? tipoOrden { get; set; }

        public List<ArticuloDto>? articulo { get; set; }
    }
}
