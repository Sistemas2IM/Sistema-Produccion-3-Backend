using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.DecisionConciliacion
{
    public class DecisionConciliacionDto
    {
        public int idDecision { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? activo { get; set; }
    }
}
