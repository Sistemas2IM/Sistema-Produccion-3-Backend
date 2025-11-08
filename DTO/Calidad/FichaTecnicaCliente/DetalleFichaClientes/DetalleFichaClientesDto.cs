using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes
{
    public class DetalleFichaClientesDto
    {
        public int? idDetalle { get; set; }

        public int? idFichaCliente { get; set; }

        public int? idVariable { get; set; }

        public string? valor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? idUnidad { get; set; }

        public string? simbolo { get; set; }

        public string? nombreUnidad { get; set; }

        public string? toleranciaPromedio { get; set; }

        public string? toleranciaMinima { get; set; }

        public string? toleranciaMaxima { get; set; }

        public string? toleranciaMedida { get; set; }

        // campos de la tabla variable
        public string? nombre { get; set; }

        public string? etiqueta { get; set; }

        public string? tipoDato { get; set; }

        public string? tolerancia { get; set; }
    }
}
