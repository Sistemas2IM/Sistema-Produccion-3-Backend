using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado
{
    public class DetalleEntregaDto
    {
        public int idDetalleEtrega { get; set; }

        public int? idEntregaPt { get; set; }

        public int? numeroFila { get; set; }

        public int? cantidadBultos { get; set; }

        public string? descripcionBultos { get; set; }

        public string? pesoBultos { get; set; }
    }
}
