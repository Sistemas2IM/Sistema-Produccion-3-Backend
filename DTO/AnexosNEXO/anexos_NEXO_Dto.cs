namespace Sistema_Produccion_3_Backend.DTO.AnexosNEXO
{
    public class anexos_NEXO_Dto
    {
        public int Id { get; set; }

        public string? TipoEntidad { get; set; }

        public int EntidadId { get; set; }

        public string? NombreArchivo { get; set; }

        public string? RutaArchivo { get; set; }

        public DateTime FechaSubida { get; set; }

        public string? SubidoPor { get; set; }

        public string? Descripcion { get; set; }
    }
}
