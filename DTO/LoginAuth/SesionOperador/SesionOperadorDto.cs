namespace Sistema_Produccion_3_Backend.DTO.LoginAuth.SesionOperador
{
    public class SesionOperadorDto
    {
        public int id { get; set; }

        public string? operador { get; set; }

        public int? maquina { get; set; }

        public string? bitacora { get; set; }

        public string? turno { get; set; }

        public bool? activa { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
