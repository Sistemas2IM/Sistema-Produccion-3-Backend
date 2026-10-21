using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Flags.FlagManual
{
    public class CrearFlagManualDto
    {
        [Required]
        public int tipoDocumento { get; set; }

        [Required]
        public int idDocumento { get; set; }

        [Required]
        public int idFlag { get; set; }

        public string? texto { get; set; }

        // Opcional: si manejas el id del usuario desde el body o lo extraes del token JWT
        public int? idUsuario { get; set; }
    }

    public class FlagConflictException : Exception
    {
        public FlagConflictException(string message) : base(message) { }
    }

    public class FlagValidationException : Exception
    {
        public FlagValidationException(string message) : base(message) { }
    }

    public class FlagNotFoundException : Exception
    {
        public FlagNotFoundException(string message) : base(message) { }
    }
}
