using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class OVDto
    {
        public int idOv { get; set; }

        [Column("oV")]
        [StringLength(200)]
        [Unicode(false)]
        public string oV1 { get; set; }

        public string codCliente { get; set; }

        public string cliente { get; set; }

        public DateTime? fechaOv { get; set; }

        public DateTime? fechaEntrega { get; set; }

        public string comentario { get; set; }

        public string vendedor { get; set; }

        public string tipoOrden { get; set; }

        public List<ArticuloDto> articulo { get; set; }

        [InverseProperty("idOvNavigation")]
        public virtual ICollection<entregasProductoTerminado> entregasProductoTerminado { get; set; } = new List<entregasProductoTerminado>();

        [InverseProperty("idOvNavigation")]
        public virtual ICollection<tarjetaOf> tarjetaOf { get; set; } = new List<tarjetaOf>();
    }
}
