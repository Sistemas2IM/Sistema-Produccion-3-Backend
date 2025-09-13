namespace Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina
{
    public class listaMaquinaCatalogoDto
    {
        public int idListaMaquina { get; set; }

        public int? idLista { get; set; }

        public int? idMaquina { get; set; }

        public bool? alterna { get; set; }

        public string? nombreLista { get; set; }
    }
}
