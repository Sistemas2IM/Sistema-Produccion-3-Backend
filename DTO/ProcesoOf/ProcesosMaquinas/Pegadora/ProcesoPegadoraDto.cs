namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Pegadora
{
    public class ProcesoPegadoraDto
    {
        public int idProcesoPegadora { get; set; }

        public int? idProceso { get; set; }

        public string? cantidadAPegar { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? material { get; set; }

        public string? calibreBase { get; set; }
    }
}
