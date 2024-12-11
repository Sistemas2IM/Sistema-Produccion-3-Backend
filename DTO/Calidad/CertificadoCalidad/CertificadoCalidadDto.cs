using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificado;
using Sistema_Produccion_3_Backend.Services;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad
{
    public class CertificadoCalidadDto
    {
        public int idCertificado { get; set; }

        public int? oF { get; set; }

        public string numeroDeLote { get; set; }

        public string numeroFactura { get; set; }

        public int? numeroOv { get; set; }

        public string cliente { get; set; }

        public string codigoProducto { get; set; }

        public string producto { get; set; }

        public string tipoDeProducto { get; set; }

        public int? cantidadEntregada { get; set; }

        public int? cantidadDespachada { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaCertificado { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaProduccion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaDespacho { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaCreacion { get; set; }

        public string observaciones { get; set; }

        public string creadoPor { get; set; }

        public int? tipoObjeto { get; set; }

        public int? version { get; set; }

        public List<DetalleCertificadoDto>? detalleCertificado { get; set; }

    }
}
