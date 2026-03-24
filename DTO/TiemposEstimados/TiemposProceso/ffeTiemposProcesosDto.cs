namespace Sistema_Produccion_3_Backend.DTO.TiemposEstimados.TiemposProceso
{
    public class ffeTiemposProcesosDto
    {
        public int idProceso { get; set; }

        public int? idOF { get; set; }

        public int? Secuencia { get; set; }

        public string? Operacion { get; set; }

        public string? Maquina { get; set; }

        public decimal? Tiempo_Est_Hrs { get; set; }

        public DateTime? Inicio_Real { get; set; }

        public DateTime? Inicio_Estimado { get; set; }

        public DateTime? Fin_Real { get; set; }

        public DateTime? Fin_Proyectado { get; set; }

        public string? Estado_Tipo { get; set; }

        public string? Alerta_Causa { get; set; }

        public DateTime? Actualizado { get; set; }
    }
}
