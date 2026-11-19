namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    public class CatalogoTaraDto
    {
        public int idTipoTara { get; set; }

        public string nombre { get; set; } = null!;

        public decimal pesoPromedio { get; set; }
    }
}
