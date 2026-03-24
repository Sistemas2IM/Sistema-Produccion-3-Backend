using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.GoogleChat.SoporteNexo
{
    public class LogSoporteNexoDto
    {
        [JsonIgnore]
        public int idLogSoporte { get; set; }

        public string? moduloOriginador { get; set; }

        public string? idReferencia { get; set; }

        public string? severidad { get; set; }

        public string? descripcion { get; set; }

        public string? user { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public string? urlReferencia { get; set; }
    }
}
