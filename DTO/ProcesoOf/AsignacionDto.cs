using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class AsignacionDto
    {
        public int idAsignacion { get; set; }

        public string? user { get; set; }

        public string? nombreUsuario { get; set; }

        public int? idProceso { get; set; }

        public int? oF { get; set; }

        public string? articuloOf {  get; set; }
    }
}
