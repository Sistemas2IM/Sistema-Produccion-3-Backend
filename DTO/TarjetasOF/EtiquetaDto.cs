using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class EtiquetaDto
    {
        public int idEtiqueta { get; set; }

        public int? idTarjetaOf { get; set; }

        public string? color { get; set; }

        public string? texto { get; set; }

        public int? secuencia { get; set; }
    }
}
