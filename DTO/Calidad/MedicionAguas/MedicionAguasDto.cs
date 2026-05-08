namespace Sistema_Produccion_3_Backend.DTO.Calidad.MedicionAguas
{
    public class MedicionAguasDto
    {
        public int idMedicionAguas { get; set; }

        public int? idFichaProceso { get; set; }

        public int? tanqueAgua { get; set; }

        public decimal? pH { get; set; }

        public decimal? conductividad { get; set; }

        public decimal? alcohol { get; set; }

        public decimal? temperatura { get; set; }
    }
}
