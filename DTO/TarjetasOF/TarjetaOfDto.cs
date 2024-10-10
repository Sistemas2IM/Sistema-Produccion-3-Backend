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

        public bool? archivado { get; set; }

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<asignacion> asignacion { get; set; } = new List<asignacion>();

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<certificadoDeCalidad> certificadoDeCalidad { get; set; } = new List<certificadoDeCalidad>();

        public List<EtiquetaDto>? etiquetaDto { get; set; }

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<fichaTecnica> fichaTecnica { get; set; } = new List<fichaTecnica>();

        public EstadoOfDto? estadoOfDto { get; set; }

        [ForeignKey("idPostura")]
        [InverseProperty("tarjetaOf")]
        public virtual posturasOf? idPosturaNavigation { get; set; }

        [InverseProperty("idTarjetaOfNavigation")]
        public virtual ICollection<procesoOf> procesoOf { get; set; } = new List<procesoOf>();

        public List<TarjetaCampoDto>? tarjetaCampoDto { get; set; }
    }
}
