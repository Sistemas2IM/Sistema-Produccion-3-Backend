namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas.Batch
{
    public class AddBatchBobinasAsignadasDto
    {
        public int? idProceso { get; set; }

        public string? codigoBobina { get; set; }

        public decimal? pesoInicial { get; set; }

        public string? codAlmacen { get; set; }
    }
}
