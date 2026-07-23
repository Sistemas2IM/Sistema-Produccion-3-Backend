using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso
{
    public class BitacoraCasoDto
    {
        public int? idEvento { get; set; }

        public int? idCasoCalidad { get; set; }

        public int? idTipoEvento { get; set; }

        //relacion
        public string? nombreTipoEvento { get; set; }

        public string? comentario { get; set; }

        public int? estadoAnterior { get; set; }

        // relacion
        public string? nombreEstadoAnterior { get; set; }

        public int? estadoNuevo { get; set; }

        // relacion
        public string? nombreEstadoNuevo { get; set; }

        public string? usuario { get; set; }

        //public string? nombreUsuario { get; set; }

        public DateTime? fecha { get; set; }
    }
}
