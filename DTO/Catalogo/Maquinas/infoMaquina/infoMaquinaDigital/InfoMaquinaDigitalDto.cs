using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaDigital
{
    public class InfoMaquinaDigitalDto
    {
        public int idMaquina { get; set; }
        public decimal? resolucionMaxima { get; set; }
        public decimal? velocidadBN { get; set; }
        public decimal? velocidadColor { get; set; }
        public string? tipoToner { get; set; }
        public string? tipoTinta { get; set; }

        public string? ripUtilizado { get; set; }
    }
}
