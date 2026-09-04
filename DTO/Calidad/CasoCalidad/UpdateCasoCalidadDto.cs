namespace Sistema_Produccion_3_Backend.DTO.Calidad.CasoCalidad
{
    public class UpdateCasoCalidadDto
    {
        public int idTipoCaso { get; set; }

        public string? origen { get; set; }

        public int idEstado { get; set; }

        public int? idSeveridad { get; set; }

        public string? descripcion { get; set; }

        public decimal? cantidadAfectada { get; set; }

        public decimal? cantidadRecuperable { get; set; }

        public decimal? cantidadNoRecuperable { get; set; }

        public int? idCategoriaDefecto { get; set; }

        public int? idSubtipoDefecto { get; set; }

        public int oF { get; set; }

        public int? idProceso { get; set; }

        public string? operador { get; set; }

        public string? registradoPor { get; set; }

        public string? responsable { get; set; }

        public string? justificacionCierre { get; set; }

        public DateTime fechaRegistro { get; set; }

        public DateTime? fechaCierre { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public int tipoReporte { get; set; }

        public bool archivado { get; set; }

        public bool cancelado { get; set; }

        public int? idCausaRaiz { get; set; }

        public int? idResolucion { get; set; }

        public int? areaResponsable { get; set; }
    }
}
