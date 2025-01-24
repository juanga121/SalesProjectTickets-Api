using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace SalesProjectTickets.Infrastructure.Backgrounds
{
    public class BackgroundTimer(ILogger<BackgroundTimer> logger, IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly ILogger<BackgroundTimer> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciado a las {time}", DateTimeOffset.Now);
            _ = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            await Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using var scope = _serviceProvider.CreateScope();
            var _ticketsServices = scope.ServiceProvider.GetRequiredService<IServiceTickets<Tickets, Guid>>();
            var tickets = await _ticketsServices.ListAllTickets();

            await Task.Run(() =>
            {
                foreach (var ticket in tickets)
                {
                    _ticketsServices.ChangeState(ticket);
                }
                _logger.LogInformation("Tickets actualizados a las {time}", DateTimeOffset.Now);
            });
        }
    }
}
*/