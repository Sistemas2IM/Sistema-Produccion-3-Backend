namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Barnizado
{
    public class ProcesoBarnizDto
    {
        public int idProcesoBarniz { get; set; }

        public int? idProcesoOf { get; set; }

        public string? cantidadPliegosImprimir { get; set; }

        public string? cantidadPliegosDemasia { get; set; }

        public string? repeticionPliegos { get; set; }

        public TimeOnly? tiempoArreglo { get; set; }

        public TimeOnly? tiempoCorrida { get; set; }

        public string? tipoMaterial { get; set; }

        public string? calibreBase { get; set; }

        public string? anchoPliego { get; set; }

        public string? largoPliego { get; set; }

        public string? tipoBarniz { get; set; }
    }
}
