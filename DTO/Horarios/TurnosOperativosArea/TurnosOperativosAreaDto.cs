namespace Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativosArea
{
    public class TurnosOperativosAreaDto
    {
        public int idTurnoArea { get; set; }

        public int? idArea { get; set; }

        public int? idTurno { get; set; }

        // Turno details

        public string? nombreTurno { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFin { get; set; }
    }
}
