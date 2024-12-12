using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol
{
    public class RolDto
    {
        public int idRol { get; set; }

        public string? nombreRol { get; set; }

        public string? descripcion { get; set; }

        public List<PermisoDto>? permisos { get; set; }
    }
}
