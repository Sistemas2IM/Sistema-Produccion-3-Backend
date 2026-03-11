using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta.EspecificacionTintas;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta
{
    public class AddFormulacionTintaDto
    {
        [JsonIgnore]
        public int idFormulacion { get; set; }

        public int? idFichaProceso { get; set; }

        public string? color { get; set; }

        public string? referencia { get; set; }

        public decimal? cie_L { get; set; }

        public decimal? cie_a { get; set; }

        public decimal? cie_b { get; set; }

        public List<AddEspecificacionTintasDto>? especificacionTintas { get; set; }
    }
}
