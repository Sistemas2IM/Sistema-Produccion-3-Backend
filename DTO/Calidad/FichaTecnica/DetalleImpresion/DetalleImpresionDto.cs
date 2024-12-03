using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion
{
    public class DetalleImpresionDto
    {
        public int idDetalleImpresion { get; set; }

        public int? idFichaTecnica { get; set; }

        public string? maquina { get; set; }

        public string? operario { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public string? resolucionPlancha { get; set; }

        public string? familiaTramadoPlancha { get; set; }

        public string? formaPunto { get; set; }

        public string? lineaturaPlancha { get; set; }

        public string? curvasPlancha { get; set; }

        public string? tipoPlancha { get; set; }

        public string? colorTiro { get; set; }

        public string? colorRetiro { get; set; }

        public string? proveedorTintas { get; set; }

        public string? presionGeneralMaquina { get; set; }

        public string? porcentajeTalco { get; set; }

        public string? tipoTalco { get; set; }

        public string? temperaturaSalida { get; set; }

        public bool? automatico { get; set; }

        public string? phAgua { get; set; }

        public string? conductividadAgua { get; set; }

        public string? alcoholAgua { get; set; }

        public string? temperaturaAgua { get; set; }

        public string? observaciones { get; set; }

        public string? formuladoPor { get; set; }

        public DateTime? fechaFormulado { get; set; }

        public string? datoInstrumento { get; set; }

        public bool? calibracionPapel { get; set; }     

        public List<DetalleBarnizDto> detalleBarnizDto { get; set; }
    }
}
