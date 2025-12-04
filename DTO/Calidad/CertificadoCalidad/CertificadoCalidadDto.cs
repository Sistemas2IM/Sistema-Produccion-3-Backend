using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad
{
    public class CertificadoCalidadDto
    {
        public int? idCertificadoCalidad { get; set; }

        public int? idFichaCliente { get; set; }

        public int? oF { get; set; }

        public string? cliente { get; set; }

        public string? producto { get; set; }

        public string? elaboradoPor { get; set; }

        public DateOnly? fechaElaboracion { get; set; }

        public int? cantidadProducida { get; set; }

        public int? cantidadDespachada { get; set; }

        public DateOnly? fechaDespacho { get; set; }

        public DateOnly? fechaProduccion { get; set; }

        public string? creditoFiscal { get; set; }

        public string? observaciones { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? actualizadoPor { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public string? remarks { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }

        public List<DetalleCertificadoCalidadDto>? detallesCertificadoCalidad { get; set; }
    }
}
