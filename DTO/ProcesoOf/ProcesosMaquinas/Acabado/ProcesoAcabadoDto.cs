using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Acabado
{
    public class ProcesoAcabadoDto
    {
        [JsonIgnore]
        public int idProcesoAcabado { get; set; }

        [JsonIgnore]
        public int? idProceso { get; set; }

        public string? cantidadMrequerido { get; set; }

        public string? cantidadDemasia { get; set; }

        public TimeOnly? tiempoArreglo { get; set; }

        public TimeOnly? tiempoCorrida { get; set; }
    }
}
