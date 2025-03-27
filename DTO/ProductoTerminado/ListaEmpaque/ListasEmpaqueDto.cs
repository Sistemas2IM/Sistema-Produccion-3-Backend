using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado.ListaEmpaque
{
    public class ListasEmpaqueDto
    {
        public string? idEmpaque { get; set; }

        public string? codigoSAP { get; set; }

        public string? empaque { get; set; }
    }
}
