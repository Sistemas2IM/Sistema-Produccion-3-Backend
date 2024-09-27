using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class SubModuloDto
    {
        public int idSubModulo { get; set; }

        public int? idModulo { get; set; }

        public string nombreSubModulo { get; set; }

        public ModuloDto modulo { get; set; }
    }
}
