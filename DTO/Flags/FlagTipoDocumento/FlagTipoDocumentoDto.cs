using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Flags.FlagTipoDocumentoDto
{
    public class FlagTipoDocumentoDto
    {
        public int? idFlag { get; set; }

        public int? tipoDocumento { get; set; }

        public int? orden { get; set; }
    }
}
