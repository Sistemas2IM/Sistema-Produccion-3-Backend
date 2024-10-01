using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class MenuDto
    {
        public int idMenu { get; set; }

        public string nombreMenu { get; set; }

        public string icono { get; set; }

        public string ruta { get; set; }
    }
}
