namespace Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida
{
    public class AddVariableUnidadMedidaDto
    {
        public int idVariable { get; set; }

        public int idUnidad { get; set; }

        public bool? predeterminada { get; set; }

        public int? orden { get; set; }
    }
}

