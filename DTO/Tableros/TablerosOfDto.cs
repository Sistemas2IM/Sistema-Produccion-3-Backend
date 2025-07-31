using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Tableros
{
    public class TablerosOfDto
    {
        public int idTablero { get; set; }

        public int? idArea { get; set; }

        public int? idMaquina { get; set; }

        public int? idFamiliaMaquina { get; set; }

        public string? nombreTablero { get; set; }

        public string? idSapMaquina { get; set; }

        public string? nombreMaquina { get; set; }

        public string? nombreAlternoMaquina { get; set; }

        public string? nombreArea { get; set; }

        public List<PosturasOfDto>? posturasOfDto { get; set; }

    }
}
