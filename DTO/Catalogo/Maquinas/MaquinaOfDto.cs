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

        public string? familiaNombre { get; set; }

        // Area desde familia maquina
        public int? idArea { get; set; }

        public string? areaNombre { get; set; }
    }
}
