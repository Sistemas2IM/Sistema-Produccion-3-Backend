using Sistema_Produccion_3_Backend.DTO.Catalogo;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class AddReporteOperadorDto
    {
        public int? idEstadoReporte { get; set; }

        public int? idTipoReporte { get; set; }

        public int? idMaquina { get; set; }

        public string? operador { get; set; }

        public DateTime? fechaDeCreacion { get; set; }

        public string? turno { get; set; }

        public int? auxiliar { get; set; }

        public decimal? velocidadEfectiva { get; set; }

        public decimal? velocidadNominal { get; set; }

        public TimeOnly? tiempoTotal { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public string? creadoPor { get; set; }

        public string? actualizadoPor { get; set; }

        public int? tipoObjeto { get; set; }
    }
}
