namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas
{
    public class ProcesoMaquinaDto
    {
        public int idMaquina { get; set; }

        public string? nombreMaquina { get; set; }

        public string? familiaNombre { get; set; }

        public string? nombreAlterno { get; set; }

        public string? descripcion { get; set; }

        public string? textoAyuda { get; set; }

        public int? velocidadObjetivo { get; set; }

        public int? velocidadMaxima { get; set; }
    }
}
