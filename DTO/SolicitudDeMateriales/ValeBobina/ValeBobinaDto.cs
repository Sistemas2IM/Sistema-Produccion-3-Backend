using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina
{
    public class ValeBobinaDto
    {
        public int idVale { get; set; }

        public string? idMaterial { get; set; }

        public decimal? pesoInicial { get; set; }

        public decimal? pesoFinal { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? loteBobinaSAP { get; set; }
    }
}
