namespace Sistema_Produccion_3_Backend.DTO.Calidad.DictamenCalidad
{
    public class DictamenCalidadDto
    {
        public int idDictamen { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }

        public bool? aplicaPreliminar { get; set; }

        public bool? aplicaFinal { get; set; }

        public int? ideEstadoSugerido { get; set; }

        public bool? activo { get; set; }
    }
}
