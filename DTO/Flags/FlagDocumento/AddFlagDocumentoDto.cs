namespace Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento
{
    public class AddFlagDocumentoDto
    {
        public int? idFlag { get; set; }

        public int? tipoDocumento { get; set; }

        public int? idDocumento { get; set; }

        public string? registradoPor { get; set; }

        public DateTime? fechaRegistro { get; set; }

        // Ignorados por el servidor: una flag manual no es propagada
        public int? idFlagOrigen { get; set; }

        public int? idFlagRaiz { get; set; }

        public string? eventoRegistro { get; set; }

        public string? texto { get; set; }
    }
}
