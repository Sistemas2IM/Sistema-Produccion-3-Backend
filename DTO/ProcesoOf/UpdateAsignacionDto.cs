using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class UpdateAsignacionDto
    {
        public string? user { get; set; }

        public int? idProceso { get; set; }
    }
}
