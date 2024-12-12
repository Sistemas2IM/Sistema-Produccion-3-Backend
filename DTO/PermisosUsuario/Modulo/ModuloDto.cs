using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo
{
    public class ModuloDto
    {
        public int idModulo { get; set; }

        public int? idMenu { get; set; }

        public string? nombreModulo { get; set; }
    }
}
