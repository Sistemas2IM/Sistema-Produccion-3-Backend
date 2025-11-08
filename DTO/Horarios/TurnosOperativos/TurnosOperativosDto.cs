namespace Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativos
{
    public class TurnosOperativosDto
    {
        public int idTurno { get; set; }

        public string? turno { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFin { get; set; }
    }
}
