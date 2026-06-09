using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.Asignacion;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesoOfSolicitud
{
    public class ProcesoOfSolicitudDto
    { 
        public int idProceso { get; set; }

        public int? oF { get; set; }

        public int? oV { get; set; }

        public int? idTablero { get; set; }

        public string? nombreTablero { get; set; }

        public int? idArea { get; set; }

        public string? nombreArea { get; set; }

        public int? idPostura { get; set; }

        public string? nombrePostura { get; set; }

        public string? lineaNegocio { get; set; }

        public string? cantRequerida { get; set; }

        public string? tipoOrden { get; set; }

        public string? unidadMedida { get; set; }

        public string? idMaquinaSAP { get; set; }
    }
}
