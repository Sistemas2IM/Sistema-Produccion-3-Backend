using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol
{
    public class AddRolDto
    {
        public int idRol { get; set; }

        public string? nombreRol { get; set; }

        public string? descripcion { get; set; }

        public bool? status { get; set; }

        public List<AddPermisoDto>? addPermisos { get; set; }
    }
}
