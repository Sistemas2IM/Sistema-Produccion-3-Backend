namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleSecado
{
    public class UpdateDetalleSecado
    {
        public int? numeroFila { get; set; }

        public int? secuencia { get; set; }

        public string? potenciaLamparaIr { get; set; }

        public string? porcentajePotenciaCalorifica { get; set; }

        public string? porcentajePotenciaAire { get; set; }

        public string? salida { get; set; }
    }
}
