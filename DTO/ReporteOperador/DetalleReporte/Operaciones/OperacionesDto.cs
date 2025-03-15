namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones
{
    public class OperacionesDto
    {
        public int idOperacion { get; set; }

        public int? familiaMaquina { get; set; }

        public string? nombreOperacion { get; set; }

        public string? tipoOperacion { get; set; }
    }
}
