using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class ModuloDto
    {
        public int idModulo { get; set; }

        public int? idMenu { get; set; }

        public string nombreModulo { get; set; }

        public MenuDto menu { get; set; }
    }
}
