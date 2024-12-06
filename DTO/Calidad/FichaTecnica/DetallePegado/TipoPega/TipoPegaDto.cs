namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado.TipoPega
{
    public class TipoPegaDto
    {
        public int idTipoPega { get; set; }

        public int? idDetallePegado { get; set; }

        public string? tipoPega1 { get; set; }

        public string? lote { get; set; }

        public string? vence { get; set; }

        public string? marca { get; set; }

        public string? proveedor { get; set; }

        public string? tolerancia { get; set; }
    }
}
