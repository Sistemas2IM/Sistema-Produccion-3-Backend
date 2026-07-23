using Sistema_Produccion_3_Backend.DTO.Calidad.SubtipoDefecto;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.CategoriaDefecto
{
    public class CategoriaDefectoDto
    {
        public int? idCategoria { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? activo { get; set; }

        public List<SubtipoDefectoDto>? subtipoDefecto { get; set; }
    }
}
