namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Preprensa
{
    public class ProcesoPreprensaDto
    {
        public int idProcesoPreprensa { get; set; }

        public int? idProceso { get; set; }

        public string? cantidadPlanchas { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }
    }
}
