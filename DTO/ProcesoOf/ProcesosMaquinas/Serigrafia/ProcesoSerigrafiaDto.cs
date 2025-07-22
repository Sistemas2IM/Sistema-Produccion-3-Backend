using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Serigrafia
{
    public class ProcesoSerigrafiaDto
    {
        [JsonIgnore]
        public int idProcesoSerigrafia { get; set; }

        [JsonIgnore]
        public int idProceso { get; set; }

        public string? cantProducir { get; set; }
    }
}
