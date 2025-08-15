using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.EstadoReporte;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class ReporteOperadorDto
    {
        public string? idReporte { get; set; }

        public int? idEstadoReporte { get; set; }

        public int? idTipoReporte { get; set; }

        public int? idMaquina { get; set; }

        public string? operador { get; set; }

        public DateTime? fechaDeCreacion { get; set; }

        public string? turno { get; set; }

        public int? auxiliar { get; set; }

        public decimal? velocidadEfectiva { get; set; }

        public decimal? velocidadNominal { get; set; }

        public TimeOnly? tiempoTotal { get; set; }

        public DateTime? ultimaActualizacion { get; set; }

        public string? creadoPor { get; set; }

        public string? actualizadoPor { get; set; }

        public int? tipoObjeto { get; set; }

        public bool? archivado { get; set; }

        public bool? cancelado { get; set; }

        public string? enviadoPor { get; set; }

        public string? aprobadoPor { get; set; }

        public string? nombreEstado { get; set; }

        public string? nombreUsuario { get; set; }

        public List<DetalleReporteDto>? detalleReporte { get; set; }

        public EstadoReporteDto? estadoReporteDto { get; set; }

        public MaquinaDto? maquinaDto { get; set; }

        public TipoReporteDto? tipoReporteDto { get; set; }
    }
}
