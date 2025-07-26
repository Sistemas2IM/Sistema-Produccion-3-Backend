namespace Sistema_Produccion_3_Backend.DTO.AnexosNEXO
{
    public class AddAnexoNEXODto
    {
        public string? TipoEntidad { get; set; }

        public int EntidadId { get; set; }

        public DateTime FechaSubida { get; set; }

        public string? SubidoPor { get; set; }

        public string? Descripcion { get; set; }

        public IFormFile? archivo { get; set; }
    }
}
