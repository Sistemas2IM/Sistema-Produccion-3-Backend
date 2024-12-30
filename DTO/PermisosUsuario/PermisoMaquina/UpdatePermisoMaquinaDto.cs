namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.PermisoMaquina
{
    public class UpdatePermisoMaquinaDto
    {
        public string? user { get; set; }

        public int? maquina { get; set; }

        public bool? asignada { get; set; }
    }
}
