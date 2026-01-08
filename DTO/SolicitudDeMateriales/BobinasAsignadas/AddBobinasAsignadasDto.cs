namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas
{
    public class AddBobinasAsignadasDto
    {
        public int? idProceso { get; set; }

        public string? codigoBobina { get; set; }

        public decimal? pesoInicial { get; set; }

        public string? codAlmacen { get; set; }
    }
}
