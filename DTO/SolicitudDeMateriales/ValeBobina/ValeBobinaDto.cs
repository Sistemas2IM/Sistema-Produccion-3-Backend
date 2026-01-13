using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina
{
    public class ValeBobinaDto
    {
        public int idVale { get; set; }

        public string? idMaterial { get; set; }

        public string? descripcionMaterial { get; set; }

        public string? proveedorMaterial { get; set; }

        public string? anchoMaterial { get; set; }

        public string? calibreMaterial { get; set; }

        public string? gramajeMaterial { get; set; }

        public decimal? pesoInicial { get; set; }

        public decimal? pesoFinal { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? loteBobinaSAP { get; set; }

        public string? proveedorBobina { get; set; }

        public decimal? anchoBobina { get; set; }

        public string? descripcionBobina { get; set; }

        public string? calibreBobina { get; set; }

        public string? gramajeBobina { get; set; }
    }
}
