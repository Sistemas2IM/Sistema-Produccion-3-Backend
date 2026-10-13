using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento
{
    public class FlagDocumentoDto
    {
        public int? idFlagDocumento { get; set; }

        public int? idFlag { get; set; }

        public int? tipoDocumento { get; set; }

        public int? idDocumento { get; set; }

        public string? texto { get; set; }

        public bool activa { get; set; }

        public int? idFlagOrigen { get; set; }

        public int? idFlagRaiz { get; set; }

        public string? eventoRegistro { get; set; }

        public string? registradoPor { get; set; }

        public DateTime fechaRegistro { get; set; }

        public DateTime? fechaRemocion { get; set; }

        public string? eventoRemocion { get; set; }

        public string? removidoPor { get; set; }

        public string? motivoRemocion { get; set; }
    }
}
