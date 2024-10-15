using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class AddOVDto
    {
        [Column("oV")]
        [StringLength(200)]
        [Unicode(false)]
        public string? oV1 { get; set; }

        public string? codCliente { get; set; }

        public string? cliente { get; set; }

        public DateTime? fechaOv { get; set; }

        public DateTime? fechaEntrega { get; set; }

        public string? comentario { get; set; }

        public string? vendedor { get; set; }

        public string? tipoOrden { get; set; }

    }
}
