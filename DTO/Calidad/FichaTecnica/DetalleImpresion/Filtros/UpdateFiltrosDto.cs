namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.Filtros
{
    public class UpdateFiltrosDto
    {
        public string? tipoFiltro { get; set; }

        public string? valorFiltro { get; set; }

        public string? filtroPolarizador { get; set; }

        public string? referenciaDeBlanco { get; set; }

        public string? condicionDeMedicion { get; set; }
    }
}
