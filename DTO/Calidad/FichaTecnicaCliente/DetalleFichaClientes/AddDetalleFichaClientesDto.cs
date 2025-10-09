namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes
{
    public class AddDetalleFichaClientesDto
    {
        public int? idFichaCliente { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
