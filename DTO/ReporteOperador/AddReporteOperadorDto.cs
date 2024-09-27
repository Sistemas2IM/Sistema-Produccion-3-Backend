using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class AddReporteOperadorDto
    {
        public int? idEstadoReporte { get; set; }

        public int? idTipoReporte { get; set; }

        public int? idMaquina { get; set; }

        public string idOperador { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? fechaDeCreacion { get; set; }

        public string turno { get; set; }

        public int? auxiliar { get; set; }

        public decimal? velocidadEfectiva { get; set; }

        public decimal? velocidadNominal { get; set; }

        public DateTime? tiempoTotal { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public string creadoPor { get; set; }

        public string actualizadoPor { get; set; }

        public int? tipoObjeto { get; set; }

        [InverseProperty("idReporteNavigation")]
        public virtual ICollection<detalleReporte> detalleReporte { get; set; } = new List<detalleReporte>();

        [ForeignKey("idEstadoReporte")]
        [InverseProperty("reportesDeOperadores")]
        public virtual estadosReporte idEstadoReporteNavigation { get; set; }

        [ForeignKey("idMaquina")]
        [InverseProperty("reportesDeOperadores")]
        public virtual maquinas idMaquinaNavigation { get; set; }

        [ForeignKey("idOperador")]
        [InverseProperty("reportesDeOperadores")]
        public virtual operador idOperadorNavigation { get; set; }

        [ForeignKey("idTipoReporte")]
        [InverseProperty("reportesDeOperadores")]
        public virtual tipoReporte idTipoReporteNavigation { get; set; }
    }
}
