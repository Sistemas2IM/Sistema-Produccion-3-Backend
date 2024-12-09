namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado.TipoPleca
{
    public class AddTipoPlecaDto
    {
        public int? idDetalleTroquelado { get; set; }

        public string? tipoDePleca { get; set; }

        public string? medida { get; set; }

        public bool? lineaRecta { get; set; }

        public bool? _45grados { get; set; }

        public string? tamanoAgujero { get; set; }

        public bool? agujero { get; set; }

        public bool? basuraMaterial { get; set; }
    }
}
