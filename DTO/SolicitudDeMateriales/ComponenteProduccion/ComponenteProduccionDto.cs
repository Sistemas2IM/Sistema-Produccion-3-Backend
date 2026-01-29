namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion
{
    public class ComponenteProduccionDto
    {
        public int idComponente { get; set; }

        public int? idProceso { get; set; }

        public string? productoOf { get; set; }

        public int? tipoSalida { get; set; }

        public int? tipoComponente { get; set; }

        public int? cantidadRequerida { get; set; }
    }
}
