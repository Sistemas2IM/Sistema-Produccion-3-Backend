namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina
{
    public class UpdateValeBobinaDto
    {
        public string? idMaterial { get; set; }

        public decimal? pesoInicial { get; set; }

        public decimal? pesoFinal { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? loteBobinaSAP { get; set; }

        public string? proveedorBobina { get; set; }

        public decimal? anchoBobina { get; set; }

        public string? descripcionBobina { get; set; }

        public string? calibreBobina { get; set; }

        public string? gramajeBobina { get; set; }

        public bool? entregaParcial { get; set; }

        public int? tipoReporte { get; set; }

        public int? estado { get; set; }

        public string? observaciones { get; set; }
    }
}
