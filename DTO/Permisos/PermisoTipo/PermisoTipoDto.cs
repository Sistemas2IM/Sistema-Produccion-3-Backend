using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Permisos.PermisoTipo
{
    public class PermisoTipoDto
    {
        public int idPermisoTipo { get; set; }

        public int? idSubModulo { get; set; }

        public string? clave { get; set; }

        public string? descripcion { get; set; }
    }
}
