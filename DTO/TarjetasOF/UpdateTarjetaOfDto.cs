namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class UpdateTarjetaOfDto
    {
        public int? idPostura { get; set; }

        public int? idEstadoOf { get; set; }

        public int oF { get; set; }

        public string nombreOf { get; set; }

        public string productoOf { get; set; }

        public string clienteOf { get; set; }

        public string descipcionOf { get; set; }

        public string vendedorOf { get; set; }

        public decimal? cantidadOf { get; set; }

        public int? posicion { get; set; }

        public DateTime? fechaUltimaActualizacion { get; set; }

        public bool? archivado { get; set; }
    }
}
