namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Troquelado
{
    public class ProcesoTroqueladoraDto
    {
        public int idProcesoTroqueladora { get; set; }

        public int? idProceso { get; set; }

        public string? idTroquel { get; set; }

        public string? cantPliegosTroquelar { get; set; }

        public string? repeticionPliegos { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? cantidadPliegosDemasia { get; set; }

        public string? indicacionImpresion { get; set; }
    }
}
