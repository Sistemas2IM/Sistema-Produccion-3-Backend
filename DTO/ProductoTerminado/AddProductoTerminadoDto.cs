using Sistema_Produccion_3_Backend.DTO.Catalogo;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;

namespace Sistema_Produccion_3_Backend.DTO.ProductoTerminado
{
    public class AddProductoTerminadoDto
    {
        public int idTarjetaOf { get; set; }

        public int idMaquina { get; set; }

        public int idOv { get; set; }

        public int idEstadoReporte { get; set; }

        public int numeroOf { get; set; }

        public string cliente { get; set; }

        public string descripcionOf { get; set; }

        public DateTime fechaEntrega { get; set; }

        public string areaEntrega { get; set; }

        public string areaRecibe { get; set; }

        public int numeroDeCorrugados { get; set; }

        public int numeroDeTarimas { get; set; }

        public int numeroDeEntrega { get; set; }

        public string creadoPor { get; set; }

        public string recibidoPor { get; set; }

        public bool entregaParcial { get; set; }

        public bool reportaSobrantes { get; set; }

        public int cantidadSobrante { get; set; }

        public int cantidadEntregadaTotal { get; set; }

        public DateTime fechaCreacion { get; set; }

        public string actualizadoPor { get; set; }

        public DateTime ultimaActualizacion { get; set; }

        public int tipoObjeto { get; set; }
    }
}
