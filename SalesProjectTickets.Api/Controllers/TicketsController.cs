using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using SalesProjectTickets.Application.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ContextsDaBa context, IValidator<Tickets> validator, IHttpContextAccessor httpContextAccessor, Cloudinary cloudinary) : ControllerBase
    {
        private readonly ContextsDaBa _context = context;
        private readonly IValidator<Tickets> _validator = validator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly Cloudinary _cloudinary = cloudinary;

        TicketsServices AddTicketsServices()
        {
            var accessor = _httpContextAccessor;
            var contextDaBa = _context;
            TicketsRepo ticketsRepo = new(contextDaBa, accessor, _cloudinary);
            TicketsServices ticketsServices = new(ticketsRepo, _validator);
            return ticketsServices;
        }

        [HttpPost]
        [Route("AddTickets")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> AddTickets([FromForm]Tickets tickets, IFormFile formFile)
        {
            try
            {
                var services = AddTicketsServices();
                await services.Add(tickets, formFile);

                return Ok(new { message = "Ticket agregado exitosamente" });

            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (PersonalException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpGet]
        [Route("ListTickets")]
        [Authorize(Policy = "AdminAndConsu")]
        public async Task<IActionResult> Get()
        {
            var services = AddTicketsServices();
            return Ok(await services.ListAllTickets());
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> EditTickets(Guid id, [FromForm]Tickets tickets, IFormFile? formFile)
        {
            var services = AddTicketsServices();
            tickets.Id = id;
            await services.Edit(tickets, formFile);
            return Ok(new { message = "Ticket actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> DeleteTickets(Guid id)
        {
            var services = AddTicketsServices();
            await services.Delete(id);
            return Ok(new { message = "Eliminado con exito" });
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Users>> Get(Guid id)
        {
            var service = AddTicketsServices();
            return Ok(await service.SelectionById(id));
        }
    }
}
