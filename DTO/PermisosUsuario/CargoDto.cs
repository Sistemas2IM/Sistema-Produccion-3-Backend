using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class CargoDto
    {
        public int idCargo { get; set; }

        public string? nombreCargo { get; set; }

        public string? descripcion { get; set; }
    }
}
