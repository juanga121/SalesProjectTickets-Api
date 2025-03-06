﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;


namespace SalesProjectTickets.Application.Services
{
    public class TicketsServices(IRepoTickets<Tickets> repoTickets, IValidator<Tickets> validator) : IServiceTickets<Tickets>
    {
        private readonly IRepoTickets<Tickets> _repoTickets = repoTickets;
        private readonly IValidator<Tickets> _validator = validator;

        public async Task<Tickets> Add(Tickets entity, IFormFile formFile)
        {
            var resultValidation = _validator.Validate(entity);
            if (!resultValidation.IsValid)
            {
                var errors = resultValidation.Errors
                    .Select(error => new ValidationsError(error.PropertyName, error.ErrorMessage))
                    .ToList();

                throw new PersonalException(errors);
            }

            entity.State = State.Disponible;
            await _repoTickets.Add(entity, formFile);
            return entity;
        }

        public async Task ChangeState(IFormFile formFile)
        {
            var allTickets = await _repoTickets.ListAllTickets();
            foreach (var ticket in allTickets)
            {
                if (ticket.Event_date < DateOnly.FromDateTime(DateTime.Now))
                {
                    ticket.State = State.Expirado;
                    await _repoTickets.Edit(ticket, formFile);
                }
            }
        }

        public async Task Delete(Guid entityID)
        {
            await _repoTickets.Delete(entityID);
        }

        public async Task Edit(Tickets entity, IFormFile formFile)
        {
            await _repoTickets.Edit(entity, formFile);
        }

        public async Task<List<Tickets>> ListAllTickets()
        {
            //await ChangeState();

            var alltickets = await _repoTickets.ListAllTickets();

            var listBy = await _repoTickets.ListByPermissions(Permissions.Administrador);

            if(listBy)
            {
                return await _repoTickets.ListAllTickets();
            }

            return alltickets.Where(tickets => tickets.State == State.Disponible).ToList();
        }

        public async Task<bool> ListByPermissions(Permissions entityPermi)
        {
            if (entityPermi == Permissions.Administrador || entityPermi == Permissions.Consumidor)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public Task<Tickets> SelectionById(Guid entity)
        {
            return _repoTickets.SelectionById(entity);
        }
    }
}
