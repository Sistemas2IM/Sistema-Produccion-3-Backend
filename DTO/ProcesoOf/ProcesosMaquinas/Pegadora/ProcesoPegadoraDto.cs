using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Pegadora
{
    public class ProcesoPegadoraDto
    {
        [JsonIgnore]
        public int idProcesoPegadora { get; set; }

        [JsonIgnore]

        public int? idProceso { get; set; }

        public string? cantidadAPegar { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? indicacion { get; set; }

        public string? tipoCierre { get; set; }
    }
}
