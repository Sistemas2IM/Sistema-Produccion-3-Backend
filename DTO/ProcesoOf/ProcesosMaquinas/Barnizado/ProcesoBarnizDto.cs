using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Barnizado
{
    public class ProcesoBarnizDto
    {
        [JsonIgnore]
        public int idProcesoBarniz { get; set; }

        [JsonIgnore]
        public int? idProceso { get; set; }

        public string? cantidadPliegosImprimir { get; set; }

        public string? cantidadPliegosDemasia { get; set; }

        public string? repeticionPliegos { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? anchoPliego { get; set; }

        public string? largoPliego { get; set; }

        public string? detalleBarniz { get; set; }

        public string? indicacionImpresion { get; set; }

        public string? barniz { get; set; }
    }
}
