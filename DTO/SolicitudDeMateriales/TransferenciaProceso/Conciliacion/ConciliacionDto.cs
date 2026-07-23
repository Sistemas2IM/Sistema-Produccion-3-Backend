using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.DecisionConciliacion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.MotivoConciliacion;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion
{
    public class ConciliacionDto
    {
        public int idConciliacion { get; set; }

        public int idPreliminar { get; set; }
        // relacion con confirmacionPreliminar
        public decimal cantidadRecibida { get; set; }
        public int? idProceso { get; set; }
        public string? maquina { get; set; }
        public int? oF { get; set; }
        public string? operador { get; set; }
        public string? nombreOperador { get; set; }
        //--------------------------------------------

        public decimal? cantidadPreliminar { get; set; }

        public decimal? cantidadTransferidaTotal { get; set; }

        public decimal? cantidadConfirmadaGenerada { get; set; }

        public decimal? diferencia { get; set; }

        public int? idMotivo { get; set; }
        // relacion con motivoConciliacion
        public MotivoConciliacionDto? motivoConciliacion { get; set; }
        //--------------------------------------------

        public int? idDecision { get; set; }
        // relacion con decisionConciliacion
        public DecisionConciliacionDto? decisionConciliacion { get; set; }
        //--------------------------------------------

        public string? comentario { get; set; }

        public string? responsable { get; set; }
        // relacion con usuario
        public string? nombreResponsable { get; set; }
        //--------------------------------------------

        public DateTime? fechaConciliacion { get; set; }

        public bool cancelado { get; set; }

        public bool archivado { get; set; }

        public ConfirmacionPreliminarDto? confirmacionPreliminar { get; set; }
    }
}
