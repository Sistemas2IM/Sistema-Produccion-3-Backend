using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.Asignacion
{
    public class AddAsignacionDto
    {
        public string user { get; set; } // id del usuario

        public int idProceso { get; set; } // id de ProcesosOf

    }
}
