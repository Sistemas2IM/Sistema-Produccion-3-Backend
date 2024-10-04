namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class UpdateDetalleReporteDto
    {
        public int? idOperacion { get; set; }

        public int? idMaterial { get; set; }

        public int? idTarjetaOf { get; set; }

        public int? idTipoCierre { get; set; }

        public DateTime? horaInicio { get; set; }

        public DateTime? horaFinal { get; set; }

        public DateTime? tiempo { get; set; }

        public string? descripcion { get; set; }

        public string? cliente { get; set; }

        public string? tiroRetiro { get; set; }

        public int? cantidadRecibida { get; set; }

        public int? cantidadProducida { get; set; }

        public int? cantidadDanada { get; set; }

        public int? cantidadSolicitada { get; set; }

        public int? cantidadProducidaMt { get; set; }

        public int? cantidadAjuste { get; set; }

        public int? cantidadNc { get; set; }

        public int? cantidadNcProducida { get; set; }

        public int? cantidadMtEnRollos { get; set; }

        public int? cantidadEnRollos { get; set; }

        public decimal? anchoBobina { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public string? observaciones { get; set; }

        public int? largoConvertido { get; set; }

        public int? anchoMm { get; set; }

        public int? largoMm { get; set; }

        public int? anchoMt { get; set; }

        public int? largoMt { get; set; }

        public decimal? ancho { get; set; }

        public decimal? alto { get; set; }

        public int? repeticiones { get; set; }

        public int? cantidadSobrante { get; set; }

        public decimal? udCorrugados { get; set; }
    }
}
