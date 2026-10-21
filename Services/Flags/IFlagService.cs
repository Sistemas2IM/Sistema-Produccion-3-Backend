using Sistema_Produccion_3_Backend.DTO.Flags.FlagDocumento;
using Sistema_Produccion_3_Backend.DTO.Flags.FlagManual;

namespace Sistema_Produccion_3_Backend.Services.Flags
{
    public interface IFlagService
    {
        Task<Dictionary<int, List<FlagDocumentoDto>>> ObtenerActivasAsync(int tipoDocumento, IReadOnlyCollection<int> idsDocumento);
        Task<FlagDocumentoDto> AsignarManualAsync(CrearFlagManualDto dto);
    }
}
