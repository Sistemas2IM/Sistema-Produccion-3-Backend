namespace Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad
{
    public class UpdateCertificadoCalidadDto
    {
        public int? idFichaCliente { get; set; }

        public int? oF { get; set; }

        public string? elaboradoPor { get; set; }

        public DateOnly fechaElaboracion { get; set; }

        public int? cantidadProducida { get; set; }

        public int? cantidadDespachada { get; set; }

        public DateOnly? fechaDespacho { get; set; }

        public DateOnly? fechaProduccion { get; set; }

        public string? creditoFiscal { get; set; }

        public string? observaciones { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
