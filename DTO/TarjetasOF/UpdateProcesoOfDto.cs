using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class UpdateProcesoOfDto
    {
        public int idProceso { get; set; }

        public int? idTarjetaOf { get; set; }

        public int? idTablero { get; set; }

        public int? idPostura { get; set; }

        public int? secuencia { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }

        public int? pliegosRecibidos { get; set; }

        public int? pliegosEntregados { get; set; }

        public int? pliegosDanados { get; set; }

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaFinalizacion { get; set; }

        public decimal? horasTotales { get; set; }

        public int? posicion { get; set; }

        public string? programadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? tipoObjeto { get; set; }

        [InverseProperty("idProcesoNavigation")]
        public virtual ICollection<detalleOperacionProceso> detalleOperacionProceso { get; set; } = new List<detalleOperacionProceso>();

        [ForeignKey("idPostura")]
        [InverseProperty("procesoOf")]
        public virtual posturasOf idPosturaNavigation { get; set; }

        [ForeignKey("idTablero")]
        [InverseProperty("procesoOf")]
        public virtual tablerosOf idTableroNavigation { get; set; }

        [ForeignKey("idTarjetaOf")]
        [InverseProperty("procesoOf")]
        public virtual tarjetaOf idTarjetaOfNavigation { get; set; }
    }
}
