using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class AddTarjetaOfDto
    {
        public int? oF { get; set; }

        public int? oV { get; set; }

        public int? idEstadoOf { get; set; }

        public string? serieOf { get; set; }

        public string? tipoDeOrden { get; set; }

        public string? nombreOf { get; set; }

        public string? productoOf { get; set; }

        public string? lineaDeNegocio { get; set; }

        public string? clienteOf { get; set; }

        public string? descipcionOf { get; set; }

        public string? vendedorOf { get; set; }

        public decimal? cantidadOf { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        public DateTime? inicio { get; set; }

        public DateTime? finalizacion { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaUltimaActualizacion { get; set; }

        public int? posicion { get; set; }

        public bool? archivada { get; set; }

        public decimal? porcentajeCompletado { get; set; }

        public int? procesosCompletados { get; set; }

        public int? procesosPendientes { get; set; }

        public int? totalProcesos { get; set; }

        public bool? cerrada { get; set; }

        public string? codArticulo { get; set; }

        public string? fsc { get; set; }

        public string? unidadMedida { get; set; }
    }
}
