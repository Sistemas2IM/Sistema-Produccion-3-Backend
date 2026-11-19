using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego;

namespace Sistema_Produccion_3_Backend.DTO.Pesaje
{
    // Conserva la estructura de lotePliegoDto y agrega los datos que necesita la tablet de pesaje
    public class PesajeLoteDto : lotePliegoDto
    {
        public string? of { get; set; }

        public string? cliente { get; set; }

        public string? materialDescripcion { get; set; }

        public decimal? gramaje { get; set; }

        public string? maquinaDestino { get; set; }

        public List<PesajeTarimaDto> tarimas { get; set; } = new();
    }
}
