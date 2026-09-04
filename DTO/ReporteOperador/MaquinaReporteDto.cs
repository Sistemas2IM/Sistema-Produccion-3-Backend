namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class MaquinaReporteDto
    {
        public int idMaquina { get; set; }
        public int? idFamilia { get; set; }
        public string? nombreFamilia { get; set; }
        public string? nombreMaquina { get; set; }
        public string? nombreCorto { get; set; }
        public string? nombreAlterno { get; set; }
        public bool? necesitaAuxiliar { get; set; }
    }
}