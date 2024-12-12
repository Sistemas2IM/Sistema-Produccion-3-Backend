using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso
{
    public class UpdatePermisoDto
    {
        [JsonIgnore]
        public int idPermiso { get; set; }

        public int? idSubModulo { get; set; }

        public int? idRol { get; set; }

        public string? descripcion { get; set; }

        public bool? canRead { get; set; }

        public bool? canAdd { get; set; }

        public bool? canEdit { get; set; }

        public bool? canDelete { get; set; }
    }
}
