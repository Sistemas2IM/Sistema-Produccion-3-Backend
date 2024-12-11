namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Vehiculos
{
    public class AddVehiculoDto
    {
        public string? nombre { get; set; }

        public string? placa { get; set; }

        public bool? activo { get; set; }

        public string? codigoActivo { get; set; }

        public bool? gPS { get; set; }
    }
}
