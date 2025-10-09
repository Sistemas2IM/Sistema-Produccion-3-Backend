namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente
{
    public class UpdateFichaTecnicaClienteDto
    {
        public int? codClienteSAP { get; set; }

        public int? codProductoSAP { get; set; }

        public string? elaboradoPor { get; set; }

        public DateOnly? fechaElaboracion { get; set; }

        public string? observaciones { get; set; }

        public int? version { get; set; }

        public bool? vigente { get; set; }

        public DateOnly? fechaVencimiento { get; set; }

        public DateTime? fechaCreacion { get; set; }
    }
}
