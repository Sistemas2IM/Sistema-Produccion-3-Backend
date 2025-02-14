using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta
{
    public class EtiquetaDto
    {
        public int idEtiqueta { get; set; }

        public string? color { get; set; }

        public string? texto { get; set; }

        public int? secuencia { get; set; }

        public int? flag { get; set; }
    }
}
