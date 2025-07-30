namespace Sistema_Produccion_3_Backend.DTO.Tableros.Posturas
{
    public class UpdateBatchPosturasOf
    {
        public int idPostura { get; set; }

        public int? idTablero { get; set; }

        public string? nombrePostura { get; set; }

        public int? secuencia { get; set; }
    }
}
