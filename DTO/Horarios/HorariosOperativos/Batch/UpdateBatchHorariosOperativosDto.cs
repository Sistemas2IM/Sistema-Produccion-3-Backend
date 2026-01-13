namespace Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos.Batch
{
    public class UpdateBatchHorariosOperativosDto
    {
        public int? idHorario { get; set; }

        public int? idArea { get; set; }

        public int? idMaquina { get; set; }

        public string? operador { get; set; }

        public DateOnly? fecha { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFin { get; set; }

        public string? estado { get; set; }

        public string? operadoresAdicionales { get; set; }

        public string? colaboradores { get; set; }
    }
}
