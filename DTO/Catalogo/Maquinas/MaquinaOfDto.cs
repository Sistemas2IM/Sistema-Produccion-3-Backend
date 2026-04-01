namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas
{
    public class MaquinaOfDto
    {
        public int idMaquina { get; set; }

        public int? idFamilia { get; set; }

        public string? nombreMaquina { get; set; }

        public string? nombreCorto { get; set; }

        public bool? necesitaAuxiliar { get; set; }

        public string? nombreAlterno { get; set; }

        public string? descripcion { get; set; }

        public string? textoAyuda { get; set; }

        public int? velocidadObjetivo { get; set; }

        public int? velocidadMaxima { get; set; }

        public int? velocidadTeorica { get; set; }

        public int? idUnidad { get; set; }

        public decimal? tamanoMaxLargoPulg { get; set; }

        public decimal? tamanoMaxAnchoPulg { get; set; }

        public decimal? tamanoMinLargoPulg { get; set; }

        public decimal? tamanoMinAnchoPulg { get; set; }

        public string? metodoImpresion { get; set; }

        public int? cantidadColoresProcesar { get; set; }

        public string? automatizacionControl { get; set; }

        public decimal? anchoImpresion { get; set; }

        public string? familiaNombre { get; set; }

        // Area desde familia maquina
        public int? idArea { get; set; }

        public string? areaNombre { get; set; }
    }
}
