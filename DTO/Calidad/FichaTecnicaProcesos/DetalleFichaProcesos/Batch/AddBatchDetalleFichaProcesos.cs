namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos.Batch
{
    public class AddBatchDetalleFichaProcesos
    {
        public int? idFichaProceso { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public int? idUnidad { get; set; }

        public string? toleranciaMedida { get; set; }

        public DateTime? fechaCracion { get; set; }
    }
}
