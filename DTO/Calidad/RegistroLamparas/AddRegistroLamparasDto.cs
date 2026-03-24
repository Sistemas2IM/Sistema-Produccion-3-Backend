namespace Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas
{
    public class AddRegistroLamparasDto
    {
        public int? idFichaProceso { get; set; }

        public int? idVariable { get; set; }

        public string? seccionMaquina { get; set; }

        public int? numeroLampara { get; set; }

        public decimal? potencia { get; set; }
    }
}
