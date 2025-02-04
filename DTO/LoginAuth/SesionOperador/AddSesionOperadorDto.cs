namespace Sistema_Produccion_3_Backend.DTO.LoginAuth.SesionOperador
{
    public class AddSesionOperadorDto
    {
        public string? operador { get; set; }

        public int? maquina { get; set; }

        public string? bitacora { get; set; }

        public string? turno { get; set; }

        public string? status { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
