using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Logistica.DetalleGira
{
    public class UpdateDetalleGiraDto
    {
        public int? oF { get; set; }

        public string? numeroDeFactura { get; set; }

        public string? otros { get; set; }

        public string? cliente { get; set; }

        public decimal? monto { get; set; }

        public string? observaciones { get; set; }
    }
}
