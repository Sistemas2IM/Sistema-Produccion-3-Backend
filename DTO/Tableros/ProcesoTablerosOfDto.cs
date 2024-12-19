using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.Tableros.Areas;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;

namespace Sistema_Produccion_3_Backend.DTO.Tableros
{
    public class ProcesoTablerosOfDto
    {
        public int idTablero { get; set; }      

        public string? nombreTablero { get; set; }

        public AreasDto? AreasDto { get; set; }

        public ProcesoMaquinaDto? Maquina { get; set; }
    }
}
