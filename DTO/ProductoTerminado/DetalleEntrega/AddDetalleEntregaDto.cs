namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado.DetalleEntrega
{
    public class AddDetalleEntregaDto
    {
        public int? idEntregaPt { get; set; }

        public int? numeroFila { get; set; }

        public int? cantidadBultos { get; set; }

        public string? descripcionBultos { get; set; }

        public string? pesoBultos { get; set; }
    }
}
