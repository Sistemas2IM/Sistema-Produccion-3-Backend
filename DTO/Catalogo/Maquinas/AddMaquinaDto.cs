namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas
{
    public class AddMaquinaDto
    {
        public int? idFamilia { get; set; }

        public string? nombreMaquina { get; set; }

        public string? nombreCorto { get; set; }

        public bool? necesitaAuxiliar { get; set; }

        public string? nombreAlterno { get; set; }

        public int? familiaId { get; set; }
    }
}
