using Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoAccionSolicitada;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.CasoCalidad
{
    public class CasoCalidadDto
    {
        public int idCasoCalidad { get; set; }

        public int idTipoCaso { get; set; }

        // relacion
        public string? nombreTipoCaso { get; set; }

        public string? origen { get; set; }

        public int idEstado { get; set; }

        // relacion
        public string? nombreEstado { get; set; }

        public int? idSeveridad { get; set; }

        // relacion
        public string? nombreSeveridad { get; set; }

        public string? descripcion { get; set; }

        public decimal? cantidadAfectada { get; set; }

        public decimal? cantidadRecuperable { get; set; }

        public decimal? cantidadNoRecuperable { get; set; }

        public int? idCategoriaDefecto { get; set; }

        // relacion
        public string? nombreCategoria { get; set; }

        public int? idSubtipoDefecto { get; set; }

        // relacion
        public string? nombreSubtipo { get; set; }

        public int oF { get; set; }

        // relaciones Of
        public string? clienteOf { get; set; }
        public string? productoOf { get; set; }

        public int? idProceso { get; set; }

        // relaciones ProcesoOf
        public string? nombreMaquina { get; set; }
        //public string? proceso { get; set; }

        public string? operador { get; set; }

        // relacion

        //public string? nombreOperador { get; set; }

        public string? registradoPor { get; set; }

        // relacion

        public string? nombreRegistradoPor { get; set; }

        public string? responsable { get; set; }

        // relacion

        public string? nombreResponsable { get; set; }

        public string? justificacionCierre { get; set; }

        public DateTime fechaRegistro { get; set; }

        public DateTime? fechaCierre { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        // relacion

        public string? nombreActualizadoPor { get; set; }

        public int tipoReporte { get; set; }

        public bool archivado { get; set; }

        public bool cancelado { get; set; }

        public List<BitacoraCasoDto>? bitacoraCaso { get; set; }

        public List<CasoAccionSolicitadaDto>? accionesSolicitadas { get; set; }
    }
}
