namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class UpdateUsuarioDto
    {
        public string? nombres { get; set; }

        public string? apellidos { get; set; }

        public string? email { get; set; }

        public int? codEmpleado { get; set; }

        public bool? status { get; set; }
    }
}
