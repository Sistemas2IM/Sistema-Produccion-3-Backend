namespace Sistema_Produccion_3_Backend.DTO.Calidad.CasoAccionSolicitada
{
    public class CasoAccionSolicitadaDto
    {
        public int? idCasoAccion { get; set; }

        public int? idCasoCalidad { get; set; }

        public int? idAccion { get; set; }

        // relacion
        public string? nombreAccion { get; set; }

        public int? idEstado { get; set; }

        // relacion
        public string? nombreEstado { get; set; }

        //public string? colorEstado { get; set; }

        public int? tipoReporte { get; set; }

        public string? comentario { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public string? responsable { get; set; }

        public DateOnly? fechaCompromiso { get; set; }

        // relacion
        //public string? nombreActualizadoPor { get; set; }
    }
}
