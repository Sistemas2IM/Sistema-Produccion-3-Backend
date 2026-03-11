namespace Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas.Batch
{
    public class AddBatchRegistroLamparas
    {
        public int? idFichaProceso { get; set; }

        public string? seccionMaquina { get; set; }

        public int? numeroLampara { get; set; }

        public decimal? potencia { get; set; }
    }
}
