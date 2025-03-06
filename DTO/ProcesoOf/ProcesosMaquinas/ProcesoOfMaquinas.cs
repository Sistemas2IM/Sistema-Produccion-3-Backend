
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas
{
    public class ProcesoOfMaquinas
    {
        public int idProceso { get; set; }

        public int? oF { get; set; }

        public int? oV { get; set; }

        public int? idTablero { get; set; }

        public int? idPostura { get; set; }

        public string? idMaterial { get; set; }

        public string? nombreTarjeta { get; set; }

        public string? cliente { get; set; }

        public string? codProd { get; set; }

        public string? vendedor { get; set; }

        public string? productoOf { get; set; }

        public string? descripcionOf { get; set; }

        public int? secuencia { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }      

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaFinalizacion { get; set; }

        public decimal? tiempoEstimado { get; set; }

        public decimal? horasTotales { get; set; }

        public int? posicion { get; set; }

        public string? programadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? tipoObjeto { get; set; }

        public bool? archivada { get; set; }

        public DateTime? fechaVencimiento { get; set; }

        public string? lineaNegocio { get; set; }

        public string? cantRequerida { get; set; }

        public string? tipoOrden { get; set; }

        public string? unidadMedida { get; set; }

        public string? fsc { get; set; }

        public string? idMaquinaSAP { get; set; }

        public string? tipoMaquinaSAP { get; set; }

        public object? DetalleProceso { get; set; }

        public PosturasOfDto? posturasOfDto { get; set; }

        public MaterialDto? materialDto { get; set; }

    }
}
