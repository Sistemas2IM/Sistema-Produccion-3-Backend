namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Impresoras
{
    public class AddBatchDetalleImpresora
    {
        public string? idReporte { get; set; }

        public int? idOperacion { get; set; }

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

        public int? cantidadNc { get; set; }

        public string? observaciones { get; set; }

        public bool? accionPorAuxiliar { get; set; }
    }
}
