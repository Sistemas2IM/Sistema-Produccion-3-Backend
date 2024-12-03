namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz
{
    public class AddDetalleBarnizDto
    {
        public int? idDetalleImpresion { get; set; }

        public string? tipoBarniz { get; set; }

        public string? nombreBarniz { get; set; }

        public string? brillo { get; set; }

        public string? gradoMedicion { get; set; }

        public string? numeroLote { get; set; }

        public string? vence { get; set; }

        public string? viscosidad { get; set; }

        public string? maquinaBarniz { get; set; }

        public string? operario { get; set; }

        public string? lote { get; set; }

        public bool? tipoRegistroPegue { get; set; }

        public bool? tipoRegistroLote { get; set; }

        public bool? tipoRegistroTotal { get; set; }

        public bool? tipoRegistroSinBuv { get; set; }

        public string? tolerancia { get; set; }
    }
}
