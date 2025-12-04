namespace Sistema_Produccion_3_Backend.DTO.Horarios.IndisponibilidadMaquinas
{
    public class UpdateIndisponibilidadMaquinasDto
    {
        public int id { get; set; }

        public int idMaquina { get; set; }

        public DateOnly fecha { get; set; }

        public string? motivo { get; set; }

        public DateTime? createdAt { get; set; }
    }
}
