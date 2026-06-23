using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaCorteConversion
{
    public class InfoMaquinaCorteConversionDto
    {
        public int idMaquina { get; set; }

        public decimal? anchoMaxBobina { get; set; }

        public decimal? diametroMaxBobina { get; set; }

        public decimal? velocidadMaxRebobinado { get; set; }

        public decimal? alturaMaxPila { get; set; }

        public decimal? presionCorte { get; set; }

        public decimal? largoMaxCorte { get; set; }

        public decimal? anchoMaxCorte { get; set; }

        public bool? sistemaCNC { get; set; }
    }
}
