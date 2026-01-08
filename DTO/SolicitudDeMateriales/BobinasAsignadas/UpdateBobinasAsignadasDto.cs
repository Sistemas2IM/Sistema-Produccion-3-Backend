namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas
{
    public class UpdateBobinasAsignadasDto
    {
        public int? idProceso { get; set; }

        public string? codigoBobina { get; set; }

        public decimal? pesoInicial { get; set; }

        public string? codAlmacen { get; set; }

        public string? codMaterial { get; set; }
    }
}
