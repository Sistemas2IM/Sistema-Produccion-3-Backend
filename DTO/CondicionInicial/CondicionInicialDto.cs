using Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial;

namespace Sistema_Produccion_3_Backend.DTO.CondicionInicial
{
    public class CondicionInicialDto
    {
        public int idCondicionInicial { get; set; }

        public int idProceso { get; set; }

        public string? operador { get; set; }

        public string? creadoPor { get; set; }

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? justificacion { get; set; }

        public bool? archivada { get; set; }

        public bool? cancelada { get; set; }

        public List<DetalleCondicionInicialDto>? detalleCondicionInicial { get; set; }
    }
}
