

using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales.ProcesosOf;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.SolicitudMateriales
{
    public class ProcesoOfSolicitudMaterialDto
    {
        public int idProceso { get; set; }

        public int? oF { get; set; }

        public int? idTablero { get; set; }

        public int? idPostura { get; set; }

        public string? nombrePostura { get; set; }

        public int? secuencia { get; set; }

        public int? posicion { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }

        public bool? archivada { get; set; }

        public string? indicador { get; set; }

        public string? tipoMaquinaSAP { get; set; }

        public string? idMaquinaSAP { get; set; }

        public int? idSolicitudMateriales { get; set; }

        public solicitudMaterialesProcesoOfDto? solicitudMateriales { get; set; }

        public List<DetalleReporteDto>? detalleProcesoOf { get; set; }
    }
}
