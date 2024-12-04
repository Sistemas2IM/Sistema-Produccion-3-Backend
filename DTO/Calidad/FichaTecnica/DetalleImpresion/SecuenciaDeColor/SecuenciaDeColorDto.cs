namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.SecuenciaDeColor
{
    public class SecuenciaDeColorDto
    {
        public int idSecuencia { get; set; }

        public int? idDetalleImpresion { get; set; }

        public int? numeroFila { get; set; }

        public int? secuencia { get; set; }

        public string tiro { get; set; }

        public string presionTiro { get; set; }

        public string densidadTiro { get; set; }

        public string porcentajeAguaTiro { get; set; }

        public string retiro { get; set; }

        public string presionRetiro { get; set; }

        public string densidadRetiro { get; set; }

        public string porcentajeAguaRetiro { get; set; }

        public string anguloColorTiro { get; set; }

        public string varianteFlujoTiro { get; set; }

        public string rodilloEntintadorTiro { get; set; }

        public string distribuciónLateralTiro { get; set; }

        public string tomadorTintaTiro { get; set; }

        public string vueltasTiro { get; set; }

        public string anguloColorRetiro { get; set; }

        public string varianteFlujoRetiro { get; set; }

        public string rodilloEntintadorRetiro { get; set; }

        public string distribuciónLateralRetiro { get; set; }

        public string tomadorTintaRetiro { get; set; }

        public string vueltasRetiro { get; set; }
    }
}
