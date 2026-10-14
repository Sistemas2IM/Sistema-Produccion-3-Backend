using Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento;

namespace Sistema_Produccion_3_Backend.Services.Flags
{
    public interface IFlagService
    {
        Task<Dictionary<int, List<FlagDocumentoDto>>> ObtenerActivasAsync(int tipoDocumento, IReadOnlyCollection<int> idsDocumento);
    }
}
