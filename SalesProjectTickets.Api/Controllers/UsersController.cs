using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Infrastructure.Repositories;

namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ContextsDaBa context, IValidator<Users> validator, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        public readonly ContextsDaBa _context = context;
        private readonly IValidator<Users> _validator = validator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        UsersServices AddServices()
        {
            var accessor = _httpContextAccessor;
            var contextsDaBa = _context;
            UserRepos userRepos = new(contextsDaBa, accessor);
            UsersServices userServices = new(userRepos, _validator);
            return userServices;
        }

        [HttpPost]
        [Route("AddUsers")]
        public async Task<IActionResult> Add([FromBody] Users users)
        {
            try
            {
                var service = AddServices();
                await service.Add(users);

                return Ok(new { message = "Usuario agregado exitosamente" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (PersonalExceptions ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        [HttpGet]
        [Route("ListUsers")]
        [Authorize(Policy = "SuperUsuario")]
        public async Task<IActionResult> Get()
        {
            var service = AddServices();
            return Ok(await service.ListAll());
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "SuperUsuario")]
        public async Task<ActionResult<Users>> Get(Guid id)
        {
            var service = AddServices();
            return Ok(await service.SelectionById(id));
        }
    }
}
