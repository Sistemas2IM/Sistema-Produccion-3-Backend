using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones.DetalleOperacionProceso;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.DetalleProceso
{
    public class DetalleProcesoOfDto
    {
        public int idDetalleOperacion { get; set; }

        public int? idProceso { get; set; }

        public int? idOperacion { get; set; }

        public int? numeroFila { get; set; }

        public int? secuencia { get; set; }

        public TimeOnly? inicio { get; set; }

        public TimeOnly? finalizacion { get; set; }

        public string? operador { get; set; }

        public int? operacion { get; set; }

        public bool? accionPorAuxiliar { get; set; }

        public string? auxiliar { get; set; }

        public int? cantidadRecibida { get; set; }

        public int? cantidadProducida { get; set; }

        public int? cantidadNc { get; set; }

        public int? idDetalleReporte { get; set; }

        public string? tiroRetiro { get; set; }

        public DateTime? fechaHora { get; set; }

        public int? bjAncho { get; set; }

        public int? bjLargo { get; set; }

        public int? largoConvertido { get; set; }

        public int? bsLargo { get; set; }

        public int? bsAncho { get; set; }

        public int? anchoBobina { get; set; }

        public int? cantAjuste { get; set; }

        public int? cantProducir { get; set; }

        public int? cantSolicitada { get; set; }

        public int? cantProducida { get; set; }

        public int? maquina { get; set; }

        public int? numAuxiliares { get; set; }

        public MaquinaDto? maquinaDto { get; set; }

        public OperacionesDto? operacionesDto { get; set; }
    }
}
