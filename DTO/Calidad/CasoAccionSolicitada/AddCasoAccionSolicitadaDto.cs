namespace Sistema_Produccion_3_Backend.DTO.Calidad.CasoAccionSolicitada
{
    public class AddCasoAccionSolicitadaDto
    {
        public int? idCasoCalidad { get; set; }

        public int? idAccion { get; set; }

        public int? idEstado { get; set; }

        public int? tipoReporte { get; set; }

        public string? comentario { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }
    }
}
