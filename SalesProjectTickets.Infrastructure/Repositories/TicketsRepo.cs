using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;

namespace SalesProjectTickets.Infrastructure.Repositories
{
    public class TicketsRepo(ContextsDaBa context, IHttpContextAccessor httpContextAccessor) : IRepoTickets<Tickets, Guid, Permissions>
    {
        private readonly ContextsDaBa _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Tickets> Add(Tickets entity)
        {
            await _context.Tickets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid entityID)
        {
            var TickestForDelete = await _context.Tickets.FirstOrDefaultAsync(tickets => tickets.Id_ticket == entityID);
            if (TickestForDelete != null)
            {
                _context.Tickets.Remove(TickestForDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(Tickets entity)
        {
            var TickestForEdit = await _context.Tickets.FirstOrDefaultAsync(tickets => tickets.Id_ticket == entity.Id_ticket);
            if (TickestForEdit != null)
            {
                TickestForEdit.Name = entity.Name;
                TickestForEdit.Description = entity.Description;
                TickestForEdit.Quantity = entity.Quantity;
                TickestForEdit.Price = entity.Price;
                TickestForEdit.Event_date = entity.Event_date;
                TickestForEdit.Event_location = entity.Event_location;
                TickestForEdit.Event_time = entity.Event_time;
                TickestForEdit.State = entity.State;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tickets>> ListAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<bool> ListByPermissions(Permissions permissions)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var permissionsClaim = user?.Claims.FirstOrDefault(c => c.Type == "Permissions");

            return await Task.FromResult(permissions switch
            {
                Permissions.Administrador => permissionsClaim?.Value == "Administrador",
                Permissions.Consumidor => permissionsClaim?.Value == "Consumidor",
                _ => false
            });
        }
    }
}
