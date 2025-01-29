using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Impresión
{
    public class ProcesoImpresoraDto
    {
        [JsonIgnore]
        public int idProcesoImpresora { get; set; }

        [JsonIgnore]
        public int? idProceso { get; set; }

        public string? cantidadPliegosImprimir { get; set; }

        public string? cantidadPliegosDemasia { get; set; }

        public string? repeticionPliegos { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? tipoMaterial { get; set; }

        public string? calibreBase { get; set; }

        public string? anchoPliego { get; set; }

        public string? largoPliego { get; set; }

        public string? tintas { get; set; }

        public string? tiro { get; set; }

        public string? retiro { get; set; }

        public string? foil { get; set; }

        public string? numerado { get; set; }

        public string? laminado { get; set; }

        public string? foilDetalle { get; set; }

        public string? barnizDetalle { get; set; }

        public string? laminadoDetalle { get; set; }

        public string? indicacionImpresion { get; set; }
    }
}
