namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    public class PesajeTarimaRegistradaDto
    {
        public int idPesaje { get; set; }

        public int correlativo { get; set; }

        public string codigoVineta { get; set; } = null!;

        public string mensaje { get; set; } = null!;
    }
}
