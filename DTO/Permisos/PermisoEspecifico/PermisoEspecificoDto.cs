using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoTipo;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico
{
    public class PermisoEspecificoDto
    {
        public int idPermisoEspecifico { get; set; }

        public int? idRol { get; set; }

        public int? idPermisoTipo { get; set; }

        public bool? habilitado { get; set; }

        public int? idSubmodulo { get; set; }

        public string? descripcion { get; set; }

        public string? clave { get; set; }
}
}
