namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class UpdateRolDto
    {
        public string? nombreRol { get; set; }

        public string? descripcion { get; set; }

        public List<UpdatePermisoDto>? updatePermisoDto {  get; set; } 
    }
}
