namespace Sistema_Produccion_3_Backend.DTO.Calidad.AccionSolicitada
{
    public class AccionSolicitadaDto
    {
        public int? idAccion { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? aplicaControlPedidos { get; set; }

        public bool? aplicaProduccion { get; set; }

        public bool? activo { get; set; }
    }
}
