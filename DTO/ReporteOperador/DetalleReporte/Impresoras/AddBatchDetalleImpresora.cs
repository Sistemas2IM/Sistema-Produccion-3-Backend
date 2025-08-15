namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Impresoras
{
    public class AddBatchDetalleImpresora
    {
        public string? idReporte { get; set; }

        public int? idOperacion { get; set; }

        public string? idMaterial { get; set; }

        public int? idProceso { get; set; }

        public int? idTipoCierre { get; set; }

        public int? oF { get; set; }

        public int? numeroFila { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFinal { get; set; }

        public TimeOnly? tiempo { get; set; }

        public string? descripcion { get; set; }

        public string? cliente { get; set; }

        public string? tiroRetiro { get; set; }

        public int? cantidadRecibida { get; set; }

        public int? cantidadProducida { get; set; }

        public int? cantidadDanada { get; set; }

        public int? cantidadSolicitada { get; set; }

        public int? cantProducida { get; set; }

        public int? cantidadAjuste { get; set; }

        public int? cantidadNc { get; set; }

        public int? cantProducir { get; set; }

        public int? cantidadMtEnRollos { get; set; }

        public int? cantidadEnRollos { get; set; }

        public int? anchoBobina { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public string? observaciones { get; set; }

        public int? largoConvertido { get; set; }

        public int? bjAncho { get; set; }

        public int? bjLargo { get; set; }

        public int? bsAncho { get; set; }

        public int? bsLargo { get; set; }

        public int? ancho { get; set; }

        public int? alto { get; set; }

        public int? repeticiones { get; set; }

        public int? cantidadSobrante { get; set; }

        public int? udCorrugados { get; set; }

        public bool? accionPorAuxiliar { get; set; }

        public DateTime? fechaHora { get; set; }

        public string? operador { get; set; }

        public int? numAuxiliares { get; set; }

        public int? maquina { get; set; }

        public DateOnly? fecha { get; set; }

        public string? actualizadoPor { get; set; }
    }
}
