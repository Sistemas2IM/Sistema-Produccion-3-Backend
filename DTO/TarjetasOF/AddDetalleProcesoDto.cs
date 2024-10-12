namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class AddDetalleProcesoDto
    {
        public int idDetalleOperacion { get; set; }

        public int? idProceso { get; set; }

        public DateTime? inicio { get; set; }

        public DateTime? pausa { get; set; }

        public DateTime? finalizacion { get; set; }

        public string? operador { get; set; }

        public int? secuencia { get; set; }

        public int? operacion { get; set; }
    }
}
