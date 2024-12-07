namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado.TipoAcabado
{
    public class AddTipoAcabadoDto
    {
        public int? idDetalleTroquelado { get; set; }

        public string tipoAcabado1 { get; set; }

        public string color { get; set; }

        public string proveedor { get; set; }

        public string codigo { get; set; }

        public string lote { get; set; }

        public string presionMaquina { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public string temperaturaLamina { get; set; }

        public string cantidad { get; set; }

        public string tipoFoil { get; set; }

        public bool? macho { get; set; }

        public bool? hembra { get; set; }
    }
}
