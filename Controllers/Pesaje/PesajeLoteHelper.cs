using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Pesaje
{
    public static class PesajeLoteHelper
    {
        public const string TipoMaquinaImpresion = "impresion";

        // El procesoOrigen del lote es el corte y no tiene OF; la OF sale de la solicitud de materiales.
        // Si la solicitud tiene varias OF se usa la única que tenga proceso de impresión; si no se puede decidir, null.
        public static async Task<int?> ObtenerOfAsync(base_nuevaContext context, lotePliego lote)
        {
            if (lote.idSolicitud.HasValue)
            {
                var ofs = await context.solicitudMaterialesOf
                    .Where(s => s.idSolicitud == lote.idSolicitud.Value)
                    .Select(s => s.oF)
                    .Distinct()
                    .ToListAsync();

                if (ofs.Count == 1)
                {
                    return ofs[0];
                }

                if (ofs.Count > 1)
                {
                    var ofsConImpresion = await context.procesoOf
                        .Where(p => p.oF != null && ofs.Contains(p.oF.Value) && p.tipoMaquinaSAP == TipoMaquinaImpresion)
                        .Select(p => p.oF!.Value)
                        .Distinct()
                        .Take(2)
                        .ToListAsync();

                    return ofsConImpresion.Count == 1 ? ofsConImpresion[0] : null;
                }
            }

            if (lote.procesoOrigen.HasValue)
            {
                return await context.procesoOf
                    .Where(p => p.idProceso == lote.procesoOrigen.Value)
                    .Select(p => p.oF)
                    .FirstOrDefaultAsync();
            }

            return null;
        }
    }
}
