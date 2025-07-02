using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Tableros.Posturas
{
    public class PosturasOfDto
    {
        public int idPostura { get; set; }

        public int? idTablero { get; set; }

        public string? nombrePostura { get; set; }

        public int? secuencia { get; set; }

        public string? tablerosOfDto { get; set; }

        //public List<ProcesoOfDto>? procesosOf { get; set; }
    }
}
