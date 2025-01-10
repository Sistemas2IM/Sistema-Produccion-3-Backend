namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado
{
    public class UpdateBatchProductoTerminado
    {
        public int idEntregaPt { get; set; }

        public int? idEstadoReporte { get; set; }

        public string? recibidoPor { get; set; }

        public DateTime? fechaRecepcion { get; set; }

        public string? actualizadoPor { get; set; }

        public DateTime? ultimaActualizacion { get; set; }
    }
}
