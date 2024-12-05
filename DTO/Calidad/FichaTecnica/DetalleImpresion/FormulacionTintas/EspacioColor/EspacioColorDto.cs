using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas.GeneralidadColor;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas.EspacioColor
{
    public class EspacioColorDto
    {
        public int idEspacioColor { get; set; }

        public int? idFormulacionTinta { get; set; }

        public string tipoEspacioColor { get; set; }

        public string valorEspacioColor { get; set; }

        public List<GeneralidadColorDto> generalidadColorDto { get; set; }
    }
}
