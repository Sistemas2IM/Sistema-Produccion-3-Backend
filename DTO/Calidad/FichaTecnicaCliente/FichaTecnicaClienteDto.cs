namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente
{
    public class FichaTecnicaClienteDto
    {
        public int? idFichaCliente { get; set; }

        public string? codClienteSAP { get; set; }

        public string? codProductoSAP { get; set; }

        public string? elaboradoPor { get; set; }

        public DateOnly? fechaElaboracion { get; set; }

        public string? observaciones { get; set; }

        public int? version { get; set; }

        public bool? vigente { get; set; }

        public DateOnly? fechaVencimiento { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
