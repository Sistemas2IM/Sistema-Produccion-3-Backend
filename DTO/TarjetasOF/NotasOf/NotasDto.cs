namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF.NotasOf
{
    public class NotasDto
    {
        public int idComentario { get; set; }

        public int? oF { get; set; }

        public string? creadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? texto { get; set; }

        public string? tipoNota { get; set; }
    }
}
