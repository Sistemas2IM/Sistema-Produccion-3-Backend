using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes
{
    public class AddDetalleFichaClientesDto
    {
        public int? idFichaCliente { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? idUnidad { get; set; }

        public string? toleranciaPromedio { get; set; }

        public string? toleranciaMinima { get; set; }

        public string? toleranciaMaxima { get; set; }

        public string? toleranciaMedida { get; set; }

    }
}
