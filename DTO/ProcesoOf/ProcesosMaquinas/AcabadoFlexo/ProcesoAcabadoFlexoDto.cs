using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.AcabadoFlexo
{
    public class ProcesoAcabadoFlexoDto
    {
        [JsonIgnore]
        public int idProcesoAcanadoFlexo { get; set; }

        [JsonIgnore]
        public int idProceso { get; set; }

        public string? cantidadMaterialSoli { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? z { get; set; }

        public string? etiquetaRollo { get; set; }

        public string? rollosTotales { get; set; }

        public string? rollosCorrugado { get; set; }

        public string? cantCorrugado { get; set; }
    }
}
