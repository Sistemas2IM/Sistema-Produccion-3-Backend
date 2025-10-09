namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes.Batch
{
    public class UpdateBatchDetalleFichaClientesDto
    {
        public int? DetalleID { get; set; }

        public int? idFichaCliente { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
