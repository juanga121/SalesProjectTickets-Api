using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ContextsDaBa context, IValidator<Tickets> validator, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly ContextsDaBa _context = context;
        private readonly IValidator<Tickets> _validator = validator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        TicketsServices AddTicketsServices()
        {
            var accessor = _httpContextAccessor;
            var contextDaBa = _context;
            TicketsRepo ticketsRepo = new(contextDaBa, accessor);
            TicketsServices ticketsServices = new(ticketsRepo, _validator);
            return ticketsServices;
        }

        [HttpPost]
        [Route("AddTickets")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> AddTickets(Tickets tickets)
        {
            var services = AddTicketsServices();
            await services.Add(tickets);

            return Ok("Ticket agregado con exito");
        }

        [HttpGet]
        [Route("ListTickets")]
        [Authorize(Policy = "AdminAndConsu")]
        public async Task<IActionResult> Get()
        {
            var services = AddTicketsServices();
            return Ok(await services.ListAllTickets());
        }

        [HttpPut("{id_ticket}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> EditTickets(Guid id_ticket, Tickets tickets)
        {
            var services = AddTicketsServices();
            tickets.Id_ticket = id_ticket;
            await services.Edit(tickets);
            return Ok("Actualizado exitosamente");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> DeleteTickets(Guid id)
        {
            var services = AddTicketsServices();
            await services.Delete(id);
            return Ok("Eliminado con exito");
        }
    }
}
