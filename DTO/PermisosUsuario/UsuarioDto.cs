using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class UsuarioDto
    {
        public string user { get; set; }

        public int? idRol { get; set; }

        public string status { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }

        public string email { get; set; }

        public DateTime? fechaDeCreacion { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public string actualizadoPor { get; set; }

        public RolDto rol { get; set; }

    }
}
