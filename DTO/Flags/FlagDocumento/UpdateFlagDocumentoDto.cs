namespace Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento
{
    public class UpdateFlagDocumentoDto
    {
        public bool? activa { get; set; }

        public DateTime? fechaRemocion { get; set; }

        public string? eventoRemocion { get; set; }

        public string? removidoPor { get; set; }

        public string? motivoRemocion { get; set; }
    }
}
