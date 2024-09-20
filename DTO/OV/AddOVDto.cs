using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class AddOVDto
    {
        public int idOv { get; set; }

        [Column("oV")]
        [StringLength(200)]
        [Unicode(false)]
        public string oV1 { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string codCliente { get; set; }

        [StringLength(200)]
        [Unicode(false)]
        public string cliente { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? fechaOv { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? fechaEntrega { get; set; }

        public string comentario { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string vendedor { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string tipoOrden { get; set; }

        [InverseProperty("idOvNavigation")]
        public virtual ICollection<articuloOv> articuloOv { get; set; } = new List<articuloOv>();

        [InverseProperty("idOvNavigation")]
        public virtual ICollection<entregasProductoTerminado> entregasProductoTerminado { get; set; } = new List<entregasProductoTerminado>();

        [InverseProperty("idOvNavigation")]
        public virtual ICollection<tarjetaOf> tarjetaOf { get; set; } = new List<tarjetaOf>();
    }
}
