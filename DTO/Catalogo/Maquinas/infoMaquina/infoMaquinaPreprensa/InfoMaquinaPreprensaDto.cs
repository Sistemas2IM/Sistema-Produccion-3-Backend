namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPreprensa
{
    public class InfoMaquinaPreprensaDto
    {
        public int idMaquina { get; set; }

        public string? tipoPlancha { get; set; }

        public decimal? resolucionCTP { get; set; }

        public string? tipoRevelado { get; set; }

        public int? tiempoProcesamiento { get; set; }

        public decimal? tamañoMaxPlancha { get; set; }
    }
}
