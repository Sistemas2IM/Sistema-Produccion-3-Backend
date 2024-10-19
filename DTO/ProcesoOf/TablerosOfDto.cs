using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class TablerosOfDto
    {
        public int idTablero { get; set; }

        public int? idArea { get; set; }

        public int? idMaquina { get; set; }

        public string? nombreTablero { get; set; }
    }
}
