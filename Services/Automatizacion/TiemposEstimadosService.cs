using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
// Asegúrate de tener los using necesarios para tus modelos y dependencias

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    public class TiemposEstimadosService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TiemposEstimadosService> _logger;

        public TiemposEstimadosService(IServiceProvider serviceProvider, ILogger<TiemposEstimadosService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 🚀 Fix mágico para no bloquear el arranque de los demás servicios ni de la API
            await Task.Yield();

            _logger.LogInformation("Servicio de Tiempos Estimados iniciado (Ciclo de 1 hora).");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<base_nuevaContext>();

                        _logger.LogInformation("Disparando SP: FFE_CalcularProcesosGlobal...");
                        await context.Database.ExecuteSqlRawAsync("EXEC FFE_CalcularProcesosGlobal", stoppingToken);

                        _logger.LogInformation("Disparando SP: FFE_CalcularCargaGlobal...");
                        await context.Database.ExecuteSqlRawAsync("EXEC FFE_CalcularCargaGlobal", stoppingToken);

                        _logger.LogInformation("Procedimientos de Tiempos Estimados ejecutados con éxito.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al ejecutar los Procedimientos Almacenados de Tiempos Estimados.");
                }

                // Pausar el servicio exactamente por 1 hora
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
