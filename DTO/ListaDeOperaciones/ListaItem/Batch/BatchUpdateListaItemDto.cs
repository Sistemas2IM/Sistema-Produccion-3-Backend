namespace Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem.Batch
{
    public class BatchUpdateListaItemDto
    {
        public int idListaItem { get; set; }

        public int? idLista { get; set; }

        public int? idOperacion { get; set; }
    }
}
