namespace Sistema_Produccion_3_Backend.DTO.ValidacionArranque
{
    public class AddValidacionArranqueDto
    {
        public int? oF { get; set; }

        public int? idProceso { get; set; }

        public int? turno { get; set; }

        public int? maquina { get; set; }

        public string? operador { get; set; }

        public string? supervisor { get; set; }

        public string? aprobadoPor { get; set; }

        public DateTime? fechaValidacion { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? barniz { get; set; }

        public decimal? ph { get; set; }

        public decimal? conductividad { get; set; }

        public decimal? alcohol { get; set; }

        public decimal? temperatura { get; set; }

        public string? observaciones { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }

        public int? tipoReporte { get; set; }

        public int? estado { get; set; }
    }
}
