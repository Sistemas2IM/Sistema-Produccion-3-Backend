namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Vehiculos
{
    public class VehiculoDto
    {
        public int idVehiculo { get; set; }

        public string? nombre { get; set; }

        public string? placa { get; set; }

        public bool? activo { get; set; }

        public string? codigoActivo { get; set; }

        public bool? gPS { get; set; }

    }
}
