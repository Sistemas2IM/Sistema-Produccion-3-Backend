namespace Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos.Batch
{
    public class AddBatchHorariosOperativosDto
    {
        public int? idArea { get; set; }

        public int? idMaquina { get; set; }

        public string? operador { get; set; }

        public DateOnly? fecha { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFin { get; set; }

        public string? estado { get; set; }
    }
}
