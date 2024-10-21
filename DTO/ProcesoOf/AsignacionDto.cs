using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class AsignacionDto
    {
        public int idAsignacion { get; set; }

        public string? user { get; set; }

        public int? idProceso { get; set; }
    }
}
