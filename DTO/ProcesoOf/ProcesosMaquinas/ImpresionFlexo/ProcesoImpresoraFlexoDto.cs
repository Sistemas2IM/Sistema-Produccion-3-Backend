using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.ImpresionFlexo
{
    public class ProcesoImpresoraFlexoDto
    {
        [JsonIgnore]
        public int idProcesoImpresoraFlexo { get; set; }

        [JsonIgnore]
        public int? idProceso { get; set; }

        public string? forma { get; set; }

        public string? radesq { get; set; }

        public string? semicorte { get; set; }

        public string? codigoMontaje { get; set; }

        public string? z { get; set; }

        public string? desarrolloPulg { get; set; }

        public string? desarrolloMM { get; set; }

        public string? anchoMaterial { get; set; }

        public string? largoMM { get; set; }

        public string? gapDesa { get; set; }

        public string? anchoMM { get; set; }

        public string? gapEje { get; set; }

        public string? layFlat { get; set; }

        public string? costuraMM { get; set; }

        public string? imposicionDesarrollo { get; set; }

        public string? imposicionEje { get; set; }

        public string? imposicionTotal { get; set; }

        public string? cantidadTintas { get; set; }

        public string? refMaterial { get; set; }

        public string? cantidadMaterialSoli { get; set; }

        public string? excedente { get; set; }

        public string? ajusteMaterial { get; set; }

        public string? areaM2 { get; set; }

        public string? sentidoSalida { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? etiquetaRollo { get; set; }

        public string? rollosTotales { get; set; }

        public string? rollosCorrugado { get; set; }

        public string? cantCorrugado { get; set; }

        public string? totalM { get; set; }

        public string? tintasFlexo { get; set; }
    }
}
