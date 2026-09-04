namespace Sistema_Produccion_3_Backend.DTO.Calidad.AccionSolicitada
{
    public class AddAccionSolicitadaDto
    {
        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public bool? aplicaControlPedidos { get; set; }

        public bool? aplicaProduccion { get; set; }

        public bool? activo { get; set; }
    }
}
