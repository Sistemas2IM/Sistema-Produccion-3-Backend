namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaTroqueladora
{
    public class InfoMaquinaTroqueladoraDto
    {
        public int idMaquina { get; set; }

        public decimal? fuerzaTroquelado_Ton { get; set; }

        public decimal? areaMaxTroquelado { get; set; }

        public decimal? areaMinTroquelado { get; set; }

        public decimal? velocidadMaxporCicloHora { get; set; }

        public decimal? sistemaStripping { get; set; }
    }
}
