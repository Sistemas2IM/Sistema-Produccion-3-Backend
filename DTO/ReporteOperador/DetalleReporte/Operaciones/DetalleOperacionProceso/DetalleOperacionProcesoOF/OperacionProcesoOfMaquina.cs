namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones.DetalleOperacionProceso.DetalleOperacionProcesoOF
{
    public class OperacionProcesoOfMaquina
    {
        public int OF { get; set; } // Corresponde a T0.[oF]
        public string? ClienteOf { get; set; }
        public string? ProductoOf { get; set; }
        public TimeOnly? Inicio { get; set; }
        public TimeOnly? Finalizacion { get; set; }
        public int? IdOperacion { get; set; }
    }
}
