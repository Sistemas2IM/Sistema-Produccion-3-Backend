using Sistema_Produccion_3_Backend.DTO.ReporteOperador;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.DetalleProceso
{
    public class DetalleProcesoOfDto
    {
        public int idDetalleOperacion { get; set; }

        public int? idProceso { get; set; }

        public int? idOperacion { get; set; }

        public int? numeroFila { get; set; }

        public int? secuencia { get; set; }

        public DateTime? inicio { get; set; }

        public DateTime? finalizacion { get; set; }

        public string? operador { get; set; }

        public int? operacion { get; set; }

        public bool? accionPorAuxiliar { get; set; }

        public OperacionesDto? operacionesDto { get; set; }
    }
}
