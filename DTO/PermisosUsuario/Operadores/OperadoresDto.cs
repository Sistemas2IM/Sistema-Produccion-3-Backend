using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.PermisoMaquina;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Operadores
{
    public class OperadoresDto
    {
        public string? user { get; set; }

        public string? nombres { get; set; }

        public string? apellidos { get; set; }

        public int? idArea { get; set; }

        public string? nombreArea { get; set; }

        public List<PermisoMaquinaAsignadaDto>? maquinasAsignadas { get; set; }
    }
}
