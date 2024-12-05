using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas.EspacioColor;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas
{
    public class FormulacionTintasDto
    {
        public int idFormulacionTinta { get; set; }

        public int? idDetalleImpresion { get; set; }

        public string color { get; set; }

        public string pintado { get; set; }

        public string referencia { get; set; }

        public string cieL { get; set; }

        public string cieA { get; set; }

        public string cieB { get; set; }

        public List<EspacioColorDto> espacioColorDto {  get; set; }
    }
}
