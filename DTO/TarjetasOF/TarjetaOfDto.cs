using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class TarjetaOfDto
    {
        public int idTarjetaOf { get; set; }

        public int? idOv { get; set; }

        public int? idPostura { get; set; }

        public int? idEstadoOf { get; set; }

        public int oF { get; set; }

        public string nombreOf { get; set; }

        public string productoOf { get; set; }

        public string clienteOf { get; set; }

        public string descipcionOf { get; set; }

        public string vendedorOf { get; set; }

        public decimal? cantidadOf { get; set; }

        public int? posicion { get; set; }

        public DateTime? fechaEntrega { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaUltimaActualizacion { get; set; }

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<asignacion> asignacion { get; set; } = new List<asignacion>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<certificadoDeCalidad> certificadoDeCalidad { get; set; } = new List<certificadoDeCalidad>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<detalleReporte> detalleReporte { get; set; } = new List<detalleReporte>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<entregasProductoTerminado> entregasProductoTerminado { get; set; } = new List<entregasProductoTerminado>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<etiqueta> etiqueta { get; set; } = new List<etiqueta>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<fichaTecnica> fichaTecnica { get; set; } = new List<fichaTecnica>();

        [ForeignKey("idEstadoOf")]
        [InverseProperty("tarjetaOf")]
        public virtual estadosOf idEstadoOfNavigation { get; set; }

        [ForeignKey("idOv")]
        [InverseProperty("tarjetaOf")]
        public virtual oV idOvNavigation { get; set; }

        [ForeignKey("idPostura")]
        [InverseProperty("tarjetaOf")]
        public virtual posturasOf idPosturaNavigation { get; set; }

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<procesoOf> procesoOf { get; set; } = new List<procesoOf>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<tarjetaCampo> tarjetaCampo { get; set; } = new List<tarjetaCampo>();
    }
}
