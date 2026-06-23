namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPegadora
{
    public class InfoMaquinaPegadoraDto
    {
        public int idMaquina { get; set; }

        public string? tipoCajaCompatible { get; set; }

        public decimal? numPuntosdePegado { get; set; }

        public string? tipoAdhesivo { get; set; }

        public decimal? velocidadMax { get; set; }
    }
}
