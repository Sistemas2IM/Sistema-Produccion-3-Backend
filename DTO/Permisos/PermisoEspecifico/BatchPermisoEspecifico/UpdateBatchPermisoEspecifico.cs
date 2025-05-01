namespace Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico.BatchPermisoEspecifico
{
    public class UpdateBatchPermisoEspecifico
    {
        public int idPermisoEspecifico { get; set; }

        public int? idRol { get; set; }

        public int? idPermisoTipo { get; set; }

        public bool? habilitado { get; set; }
    }
}
