using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Operadores
{
    public class PermisoMaquinaAsignadaDto
    {
        [JsonIgnore]
        public int idPermisoMaquina { get; set; }

        [JsonIgnore]
        public string? user { get; set; }

        public int? maquina { get; set; }

        public string? nombreMaquina { get; set; }

        public bool? asignada { get; set; }
    }
}
