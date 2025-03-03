using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.MaterialOf
{
    public class MaterialOfDto
    {
        public string? idMaterial { get; set; }

        public string? nombreMaterial { get; set; }

        public string? tipoMaterial { get; set; }

        public string? calibre { get; set; }

        public string? _base { get; set; }
    }
}
