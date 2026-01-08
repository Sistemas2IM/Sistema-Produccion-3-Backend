namespace Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos
{
    public class HorariosOperativosDto
    {
        public int? idHorario { get; set; }

        public int? idArea { get; set; }

        public string? nombreArea { get; set; }

        public int? idMaquina { get; set; }

        public string? nombreMaquina { get; set; }

        public string? operador { get; set; }

        public string? nombreOperador { get; set; }

        public DateOnly? fecha { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFin { get; set; }

        public string? estado { get; set; }

        public string? operador2 { get; set; }

        public string? operador3 { get; set; }
    }
}
