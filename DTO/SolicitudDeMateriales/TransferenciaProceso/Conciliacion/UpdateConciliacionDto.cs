namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion
{
    public class UpdateConciliacionDto
    {
        public int idPreliminar { get; set; }

        public decimal? cantidadPreliminar { get; set; }

        public decimal? cantidadTransferidaTotal { get; set; }

        public decimal? cantidadConfirmadaGenerada { get; set; }

        public decimal? diferencia { get; set; }

        public int? idMotivo { get; set; }

        public int? idDecision { get; set; }

        public string? comentario { get; set; }

        public string? responsable { get; set; }

        public DateTime? fechaConciliacion { get; set; }

        public bool cancelado { get; set; }

        public bool archivado { get; set; }
    }
}
