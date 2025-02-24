using FluentValidation;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;


namespace SalesProjectTickets.Application.Services
{
    public class TicketsServices(IRepoTickets<Tickets, Guid, Permissions> repoTickets, IValidator<Tickets> validator) : IServiceTickets<Tickets, Guid, Permissions>
    {
        private readonly IRepoTickets<Tickets, Guid, Permissions> _repoTickets = repoTickets;
        private readonly IValidator<Tickets> _validator = validator;

        public async Task<Tickets> Add(Tickets entity)
        {
            var resultValidation = _validator.Validate(entity);
            if (!resultValidation.IsValid)
            {
                var errors = resultValidation.Errors
                    .Select(error => new ValidationsError(error.PropertyName, error.ErrorMessage))
                    .ToList();

                throw new PersonalExceptions(errors);
            }

            entity.State = State.Disponible;
            await _repoTickets.Add(entity);
            return entity;
        }

        public async Task ChangeState()
        {
            var allTickets = await _repoTickets.ListAllTickets();
            foreach (var ticket in allTickets)
            {
                if (ticket.Event_date < DateOnly.FromDateTime(DateTime.Now))
                {
                    ticket.State = State.Expirado;
                    await _repoTickets.Edit(ticket);
                }
            }
        }

        public async Task Delete(Guid entityID)
        {
            await _repoTickets.Delete(entityID);
        }

        public async Task Edit(Tickets entity)
        {
            await _repoTickets.Edit(entity);
        }

        public async Task<List<Tickets>> ListAllTickets()
        {
            await ChangeState();

            var alltickets = await _repoTickets.ListAllTickets();

            var listBy = await _repoTickets.ListByPermissions(Permissions.Administrador);

            if(listBy)
            {
                return await _repoTickets.ListAllTickets();
            }

            return alltickets.Where(tickets => tickets.State == State.Disponible).ToList();
        }

        public async Task<bool> ListByPermissions(Permissions permissions)
        {
            if (permissions == Permissions.Administrador || permissions == Permissions.Consumidor)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
