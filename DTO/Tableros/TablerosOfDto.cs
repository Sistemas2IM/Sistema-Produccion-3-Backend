using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Tableros
{
    public class TablerosOfDto
    {
        public int idTablero { get; set; }

        public int? idArea { get; set; }

        public int? idMaquina { get; set; }

        public string? nombreTablero { get; set; }

        public virtual areas? idAreaNavigation { get; set; }

        public virtual maquinas? idMaquinaNavigation { get; set; }

        public List<PosturasOfDto>? posturasOfDto { get; set; }

    }
}
