using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas.GeneralidadColor
{
    public class GeneralidadColorDto
    {
        public int idGeneralidad { get; set; }

        public int? idEspacioColor { get; set; }

        public string descripcionTinta { get; set; }

        public string proveedorTinta { get; set; }

        public string porcentajeTinta { get; set; }
    }
}
