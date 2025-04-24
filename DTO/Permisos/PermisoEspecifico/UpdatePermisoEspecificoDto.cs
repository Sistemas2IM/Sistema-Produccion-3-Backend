namespace Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico
{
    public class UpdatePermisoEspecificoDto
    {
        public int? idRol { get; set; }

        public int? idPermisoTipo { get; set; }

        public bool? habilitado { get; set; }
    }
}
