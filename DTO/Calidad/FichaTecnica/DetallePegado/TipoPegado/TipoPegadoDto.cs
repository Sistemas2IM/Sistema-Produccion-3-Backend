namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado.TipoPegado
{
    public class TipoPegadoDto
    {
        public int idTipoPegado { get; set; }

        public int? idDetallePegado { get; set; }

        public string tipoPegado1 { get; set; }

        public bool? disco { get; set; }

        public bool? pistola { get; set; }
    }
}
