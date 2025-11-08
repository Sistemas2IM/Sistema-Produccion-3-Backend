using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida
{
    public class VariableUnidadMedidaDto
    {
        [JsonIgnore]
        public int idVariable { get; set; }

        public int idUnidad { get; set; }

        public bool? predeterminada { get; set; }

        public int? orden { get; set; }

        public string? nombreUnidad { get; set; }

        public string? simbolo { get; set; }
    }
}
