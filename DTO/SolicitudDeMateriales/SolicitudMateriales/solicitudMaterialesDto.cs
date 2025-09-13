namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales
{
    public class solicitudMaterialesDto
    {
        public int idSolicitud { get; set; }

        public string? trabajo { get; set; }

        public int? tipoPreparacion { get; set; }

        public DateTime? fechaSolicitud { get; set; }

        public string? idMaterialSAP { get; set; }

        public decimal? anchoHoja { get; set; }

        public decimal? largoHoja { get; set; }

        public int? cantidadPorCortar { get; set; }

        public int? estado { get; set; }

        public int? tiraje { get; set; }

        public int? idMaquina { get; set; }

        public decimal? anchoPliego { get; set; }

        public decimal? largoPliego { get; set; }

        public int? pliegosPorHoja { get; set; }

        public int? repeticionesPorPliego { get; set; }

        public int? excedente { get; set; }

        public string? solicitadoPor { get; set; }
    }
}
