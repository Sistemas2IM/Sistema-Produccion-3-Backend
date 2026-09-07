namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF.HistorialVencimientoOf
{
    public class HistorialVencimientoOfDto
    {
        public int idHistorial { get; set; }

        public int oF { get; set; }

        public DateTime fechaVencimientoAnterior { get; set; }

        public DateTime fechaVencimientoNueva { get; set; }

        public string? origen { get; set; }

        public string? registradoPor { get; set; }

        public string? nombreRegistradoPor { get; set; }

        public string? comentario { get; set; }

        public DateTime fechaRegistro { get; set; }

        public int? motivoCambio { get; set; }

        public string? nombreMotivoCambio { get; set; }
    }
}
