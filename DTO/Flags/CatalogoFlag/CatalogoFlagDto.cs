using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Flags.CatalogoFlag
{
    public class CatalogoFlagDto
    {
        public int idFlag { get; set; }

        [Required]
        [StringLength(30)]
        public string codigo { get; set; }

        [Required]
        [StringLength(80)]
        public string nombre { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        public string efecto { get; set; }

        public string modoAsignacion { get; set; }

        public bool requiereTexto { get; set; }

        public bool permiteQuitarManual { get; set; }

        public string color { get; set; }

        public bool activo { get; set; }
    }
}
