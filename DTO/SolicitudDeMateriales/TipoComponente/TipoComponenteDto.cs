namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TipoSemielaborados
{
    public class TipoComponenteDto
    {
        public int idTipoComponente { get; set; }

        public string? codigo { get; set; }

        public string? descripcion { get; set; }

        public int? unidadBase { get; set; }

        public bool? esFinal { get; set; }
    }
}
