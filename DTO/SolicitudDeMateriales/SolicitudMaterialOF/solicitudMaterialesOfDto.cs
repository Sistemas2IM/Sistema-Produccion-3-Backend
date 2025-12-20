using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF
{
    public class solicitudMaterialesOfDto
    {
        public int oF { get; set; }

        public string? cliente { get; set; }

        public string? descripcionOf { get; set; }

        public string? cantidadOf { get; set; }

        public DateTime? fechaEntrega { get; set; }

        [JsonIgnore]
        public int idSolicitud { get; set; }

        public int? cantidadAsignada { get; set; }
    }
}
