using Sistema_Produccion_3_Backend.Services;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.CorridaCombinada
{
    public class CorridaCombinadaDto
    {
        public int idRelacion { get; set; }

        public int? maestro { get; set; }

        public int? subordinado { get; set; }

        public string? oF { get; set; } // de subordinado / proceso of

        public string? productoOf { get; set; } // de subordinado / proceso of

        public string? cantOf { get; set; } // de subordinado / proceso of

        //[JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaVencmiento { get; set; } // de subordinado / proceso of

        public string? serie { get; set; } // de subordinado / proceso of
    }
}
