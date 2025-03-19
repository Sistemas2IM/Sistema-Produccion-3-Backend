namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador.PausaMaquina
{
    public class UpdatePausasMaquinaDto
    {
        public int? maquina { get; set; }

        public string? usuario { get; set; }

        public DateTime? inicio { get; set; }

        public DateTime? fin { get; set; }

        public string? bitacoraId { get; set; }

        public string? tipoPausa { get; set; }

        public int? idOperacion { get; set; }
    }
}
