using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones
{
    public class listaDeOperacionesDto
    {
        public int idLista { get; set; }

        public string? nombreLista { get; set; }

        public string? descripcion { get; set; }

        public List<listaItemDto>? items { get; set; }
    }
}
