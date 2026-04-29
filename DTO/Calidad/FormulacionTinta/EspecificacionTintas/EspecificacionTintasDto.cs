namespace Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta.EspecificacionTintas
{
    public class EspecificacionTintasDto
    {
        public int idEspecificacion { get; set; }

        public int? idFormulacion { get; set; }

        public string? descripcion { get; set; }

        public string? proveedor { get; set; }

        public decimal? porcentajeTinta { get; set; }

        public string? loteTinta { get; set; }
    }
}
