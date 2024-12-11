namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado.ContenidoEntrega
{
    public class AddContenidoEntregaDto
    {
        public int? numeroFila { get; set; }

        public int? cantidadPlanificada { get; set; }

        public int? cantidadEntregada { get; set; }

        public string? producto { get; set; }

        public string? descripcion { get; set; }

        public string? codigoProducto { get; set; }
    }
}
