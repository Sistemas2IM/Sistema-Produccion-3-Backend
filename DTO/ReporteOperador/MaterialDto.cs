using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class MaterialDto
    {
        public string? idMaterial { get; set; }

        public string? nombreMaterial { get; set; }

        public string? calibre { get; set; }

        [JsonPropertyName("base")]
        public string? _base { get; set; }

        public string? marca { get; set; }
    }
}
