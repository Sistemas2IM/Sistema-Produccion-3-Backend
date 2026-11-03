namespace Sistema_Produccion_3_Backend.Services.Flags
{
    // 400
    public class FlagValidationException : Exception
    {
        public FlagValidationException(string message) : base(message) { }
    }

    // 404
    public class FlagNotFoundException : Exception
    {
        public FlagNotFoundException(string message) : base(message) { }
    }

    // 409
    public class FlagConflictException : Exception
    {
        public FlagConflictException(string message) : base(message) { }
    }
}
