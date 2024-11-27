namespace Sistema_Produccion_3_Backend.DTO.Tableros
{
    public class AreasTablerosDto
    {
        public int idArea { get; set; }

        public string? nombreArea { get; set; }

        public List<TablerosOfDto>? tablerosOfDto { get; set; }
    }
}
