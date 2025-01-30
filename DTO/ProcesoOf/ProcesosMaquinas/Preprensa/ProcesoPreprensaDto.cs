namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Preprensa
{
    public class ProcesoPreprensaDto
    {
        public int idProcesoPreprensa { get; set; }

        public int? idProceso { get; set; }

        public string? cantidadPlanchas { get; set; }

        public string? tiempoArreglo { get; set; }

        public string? tiempoCorrida { get; set; }

        public string? tiro { get; set; }

        public string? retiro { get; set; }

        public string? foil { get; set; }

        public string? numerado { get; set; }

        public string? laminado { get; set; }

        public string? medidaProd {  get; set; }
    }
}
