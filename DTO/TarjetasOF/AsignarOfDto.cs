namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{

    public class AsignarOfDto
    {
        public int? idDisenador { get; set; }
        public int? idTarjetaOf { get; set; }

        // Incluye solo los datos necesarios de las tablas relacionadas, por ejemplo:
        public string? NombreDisenador { get; set; } // Solo una propiedad simple
        public string? TarjetaOfDescripcion { get; set; } // Solo una propiedad de tarjetaOf
    }
}


