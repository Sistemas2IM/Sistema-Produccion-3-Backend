namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado
{
    public class AddDetallePegadoDto
    {
        public int? idFichaTecnica { get; set; }

        public string? maquina { get; set; }

        public string? operario { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public decimal? presionBandaColectora { get; set; }

        public string? pegaDisco { get; set; }

        public string? pegaPistola { get; set; }

        public int? bandasATransferir { get; set; }

        public string? velocidadPegado { get; set; }

        public string? tipoAcetato { get; set; }

        public string? espesorAcetato { get; set; }

        public string? anchoAcetato { get; set; }

        public string? largoAcetato { get; set; }

        public string? toleranciaAcetato { get; set; }

        public string? anchoPega { get; set; }

        public string? largoPega { get; set; }

        public string? toleranciaPega { get; set; }

        public string? alimentacion { get; set; }

        public string? tipoCorrugado { get; set; }

        public int? unidadesCorrugado { get; set; }

        public bool? fajado { get; set; }

        public int? unidadesFajadas { get; set; }

        public string? observaciones { get; set; }
    }
}
