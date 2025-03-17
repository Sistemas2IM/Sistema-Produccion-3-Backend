namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones
{
    public class UpdateOperacionesDto
    {
        public int? familiaMaquina { get; set; }

        public string? nombreOperacion { get; set; }

        public string? tipoOperacion { get; set; }

        public string prefijo { get; set; }
    }
}
