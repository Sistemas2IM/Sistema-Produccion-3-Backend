namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf
{
    public class AddBatchProcesoDto
    {
        public int? oF { get; set; }

        public int? oV { get; set; }

        public int? idTablero { get; set; }

        public int? idPostura { get; set; }

        public string? idMaterial { get; set; }

        public string? nombreTarjeta { get; set; }

        public string? productoOf { get; set; }

        public string? descripcionOf { get; set; }

        public int? secuencia { get; set; }

        public bool? completada { get; set; }

        public bool? bloqueada { get; set; }

        public int? pliegosRecibidos { get; set; }

        public int? pliegosEntregados { get; set; }

        public int? pliegosDanados { get; set; }

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaFinalizacion { get; set; }

        public decimal? tiempoEstimado { get; set; }

        public decimal? horasTotales { get; set; }

        public int? posicion { get; set; }

        public string? programadoPor { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? tipoObjeto { get; set; }

        public bool? archivada { get; set; }

        public DateTime? fechaVencimiento { get; set; }
    }
}
