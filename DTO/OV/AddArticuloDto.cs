using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class AddArticuloDto
    {
        public int? oV { get; set; }

        public string articulo { get; set; }

        public string unidad { get; set; }

        public string detalleArticulo { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? precioUnidad { get; set; }

        [Unicode(false)]
        public string lineaDeNegocio { get; set; }

        [Unicode(false)]
        public string departamento { get; set; }

        [ForeignKey("idOv")]
        [InverseProperty("articuloOv")]
        public virtual oV? idOvNavigation { get; set; }
    }
}
