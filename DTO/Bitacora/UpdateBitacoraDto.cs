namespace Sistema_Produccion_3_Backend.DTO.Bitacora
{
    public class UpdateBitacoraDto
    {
        public string? usuario { get; set; }

        public int? idObjeto { get; set; }

        public int? tipoObjeto { get; set; }

        public DateTime? fechaActualizacion { get; set; }
    }
}
