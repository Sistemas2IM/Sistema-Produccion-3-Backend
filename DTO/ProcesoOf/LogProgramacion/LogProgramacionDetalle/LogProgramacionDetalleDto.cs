namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle
{
    public class LogProgramacionDetalleDto
    {
        public int? idLogProgramacionDetalle { get; set; }

        public int? idLogProgramacion { get; set; }

        public int? idProceso { get; set; }

        public string? accion { get; set; }

        public int? estadoAnterior { get; set; }

        public int? estadoNuevo { get; set; }

        public int? posicionAnterior { get; set; }

        public int? posicionNueva { get; set; }
    }
}
