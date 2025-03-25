namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateMaquina
{
    public class UpdateMaquinaProcesoOf
    {
        public int idProceso { get; set; }

        public int? idTablero { get; set; }

        public int? idPostura { get; set; }

        public int? posicion { get; set; }

        public DateTime? fechaActualización { get; set; }

        public string? comentario { get; set; }

        public string? actualizadoPor { get; set; }

        public bool? muestra { get; set; }
    }
}
