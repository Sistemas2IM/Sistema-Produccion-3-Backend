namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales
{
    public class UpdateSolicitudMaterialesDto
    {
        public string? trabajo { get; set; }

        public string? tipoPreparacion { get; set; }

        public DateTime? fechaSolicitud { get; set; }

        public string? idMaterialSAP { get; set; }

        public decimal? anchoHoja { get; set; }

        public decimal? largoHoja { get; set; }

        public int? cantidadPorCortar { get; set; }

        public string? estado { get; set; }

        public int? tiraje { get; set; }

        public int? idMaquina { get; set; }

        public decimal? anchoPliego { get; set; }

        public decimal? largoPliego { get; set; }

        public int? pliegosPorHoja { get; set; }

        public int? repeticionesPorPliego { get; set; }

        public int? excedente { get; set; }

        public string? solicitadoPor { get; set; }

        public int? idSap { get; set; }

        public decimal? anchoHojaPulg { get; set; }

        public decimal? anchoPliegoPulg { get; set; }

        public decimal? largoPliegoPulg { get; set; }

        public decimal? areaApulg { get; set; }

        public decimal? areaA { get; set; }

        public decimal? areaBpulg { get; set; }

        public decimal? areaB { get; set; }

        public decimal? largoHojaPulg { get; set; }

        public string? compra { get; set; }

        public string? comentarios { get; set; }

        public string? interna { get; set; }

        public string? programada { get; set; }

        public string? rerefencia { get; set; }

        public string? direccionCorte { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }

        public string? granoHoja { get; set; }

        public string? subFamiliaSap { get; set; }

        public string? tipoOperacion { get; set; }

        public bool? IncluyeProceso { get; set; }

        public string? descripcion { get; set; }

        public int? posicion { get; set; }
    }
}
