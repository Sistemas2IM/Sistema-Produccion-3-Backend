namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos
{
    public class AddFichaTecnicaProcesosDto
    {
        public int? oF { get; set; }

        public int? idProceso { get; set; }

        public int? maquina { get; set; }

        public int? estado { get; set; }

        public int? tipoProceso { get; set; }

        public string? operador { get; set; }

        public string? observaciones { get; set; }

        public decimal? presionGeneral { get; set; }

        public int? version { get; set; }

        public bool? vigente { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? creadoPor { get; set; }

        public DateOnly? fechaFormulacion { get; set; }

        public string? formuladorTinta { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public int? tipoReporte { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }
    }
}
