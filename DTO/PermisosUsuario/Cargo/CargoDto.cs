using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Cargo
{
    public class CargoDto
    {
        public int idCargo { get; set; }

        public string? nombreCargo { get; set; }

        public string? descripcion { get; set; }

        [JsonIgnore]
        public List<UsuarioDto>? usuarios { get; set; }
    }
}
