using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;

namespace Sistema_Produccion_3_Backend.DTO.LoginAuth
{
    public class AreasUsuariosDto
    {
        public int idArea { get; set; }

        public string? nombreArea { get; set; }

        public List<UsuarioDto>? usuarios { get; set; }
    }
}
