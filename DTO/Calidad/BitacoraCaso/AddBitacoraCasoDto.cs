namespace Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso
{
    public class AddBitacoraCasoDto
    {
        public int? idCasoCalidad { get; set; }

        public int? idTipoEvento { get; set; }

        public string? comentario { get; set; }

        public int? estadoAnterior { get; set; }

        public int? estadoNuevo { get; set; }

        public string? usuario { get; set; }

        public DateTime? fecha { get; set; }

        public int? idAnexo { get; set; }

        public int? idDictamen { get; set; }

        public string? acciones { get; set; }
    }
}
