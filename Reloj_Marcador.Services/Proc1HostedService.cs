using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    /// <summary>
    /// Servicio hospedado que ejecuta PROC1 automáticamente cada día a una hora configurable (por defecto 23:59).
    /// </summary>
    public class Proc1HostedService : BackgroundService
    {
        private readonly ILogger<Proc1HostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _horaEjecucion;

        public Proc1HostedService(
            ILogger<Proc1HostedService> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _horaEjecucion = TimeSpan.Parse(config["Proc1:HoraEjecucion"] ?? "23:59:00");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio PROC1 automático iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var ahora = DateTime.Now;
                    var proximaEjecucion = DateTime.Today.Add(_horaEjecucion);

                    if (ahora > proximaEjecucion)
                        proximaEjecucion = proximaEjecucion.AddDays(1);

                    var tiempoEspera = proximaEjecucion - ahora;

                    _logger.LogInformation($"Proxima ejecución PROC1 programada para: {proximaEjecucion:yyyy-MM-dd HH:mm:ss}");

                    await Task.Delay(tiempoEspera, stoppingToken);

                    // Crear un nuevo scope cada ejecución
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var proc1Service = scope.ServiceProvider.GetRequiredService<Proc1Service>();

                        var fecha = DateTime.Today;
                        await proc1Service.EjecutarAsync(fecha, fecha);

                        _logger.LogInformation($"PROC1 ejecutado automáticamente para el {fecha:yyyy-MM-dd}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la ejecución automática de PROC1.");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
