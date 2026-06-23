using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaFlexografia
{
    public class InfoMaquinaFlexografiaDto
    {
        public int idMaquina { get; set; }

        public decimal? anchoMaxImpresion { get; set; }

        public int? repeticionMaxima { get; set; }

        public int? repeticionMinima { get; set; }

        public int? numEstaciones { get; set; }

        public string? tipoAnilox { get; set; }

        public string? tipoSecado { get; set; }

        public bool? impresionFrenteyVuelta { get; set; }

        public decimal? temperaturaMax { get; set; }

        public decimal? temperaturaMin { get; set; }

        public decimal? presionMax { get; set; }

        public string? tipoPelicula { get; set; }
    }
}
