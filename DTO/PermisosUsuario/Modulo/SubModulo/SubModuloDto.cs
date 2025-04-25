using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoTipo;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo
{
    public class SubModuloDto
    {
        public int idSubModulo { get; set; }

        public int? idModulo { get; set; }

        public string? nombreSubModulo { get; set; }

        public ModuloDto? modulo { get; set; }

        public List<PermisoTipoDto>? permisoTipoDto { get; set; }
    }
}
