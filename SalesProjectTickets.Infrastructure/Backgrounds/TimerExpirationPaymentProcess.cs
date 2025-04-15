using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Infrastructure.Backgrounds
{
    public class TimerExpirationPaymentProcess(ILogger<TimerExpirationPaymentProcess> logger, IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly ILogger<TimerExpirationPaymentProcess> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Timer? _timer;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ha comenzado el servicio de timer para pagos");

            _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            await Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                _logger.LogInformation("Iniciando cambio de estado");
                PurchaseHistory purchaseHistory = new();
                var paymentServices = scope.ServiceProvider.GetRequiredService<IServicePaymentProcess>();
                await paymentServices.ChangeStatusAfter(purchaseHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de timer");
            }
            finally
            {
                _timer?.Change(TimeSpan.FromMinutes(5), TimeSpan.FromMilliseconds(-1));
            }
        }
    }
}
