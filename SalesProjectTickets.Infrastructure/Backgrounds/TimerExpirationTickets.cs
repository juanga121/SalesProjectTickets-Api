using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Infrastructure.Backgrounds
{
    public class TimerExpirationTickets(ILogger<TimerExpirationTickets> logger, IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly ILogger<TimerExpirationTickets> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Timer? _timer;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ha comenzado el servicio de timer");

            //var nextRunTime = GetNextRunTime();
            //var initialDelay = nextRunTime - DateTime.Now;

            _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            await Task.CompletedTask;
        }

        private DateTime GetNextRunTime()
        {
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 12, 30, 0);
            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            return nextRunTime;
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                _logger.LogInformation("Iniciando cambio de estado");
                var ticketsServices = scope.ServiceProvider.GetRequiredService<IServiceTickets<Tickets>>();
                await ticketsServices.ChangeState();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de timer");
            }
            finally
            {
                //var nextRunTime = GetNextRunTime();
                //var delay = nextRunTime - DateTime.Now;
                _timer?.Change(TimeSpan.FromMinutes(5), TimeSpan.FromMilliseconds(-1));
            }
        }
    }
}
