namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado.DetalleEntrega
{
    public class AddDetalleEntregaDto
    {
        public int? idEntregaPt { get; set; }

        public int? numeroFila { get; set; }

        public int? numeroOF { get; set; }

        public string cliente { get; set; }

        public decimal? cantidad { get; set; }

        public string codArticuloSAP { get; set; }

        public string tipoEmpaque { get; set; }

        public int? bultos { get; set; }

        public decimal? peso { get; set; }

        public string nombreArticulo { get; set; }

        public decimal? cantidadTotal { get; set; }
    }
}
