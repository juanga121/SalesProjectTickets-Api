using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesProjectTickets.Application.DTOs;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Infrastructure.Provider;


namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IServiceLogin<LoginDTO> _servicesLogin, TokenProvider tokenProvider, IMapper mapper) : ControllerBase
    {
        private readonly IServiceLogin<LoginDTO> servicesLogin = _servicesLogin;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginUsers)
        {
            try
            {
                var users = await servicesLogin.Login(loginUsers);
                if (users == null)
                {
                    return BadRequest(new
                    {
                        Message = "Credenciales incorrectas"
                    });
                }

                var loginEntity = _mapper.Map<LoginUsers>(users);
                var token = tokenProvider.GenerateToken(loginEntity);

                return Ok(new
                {
                    Token = token,
                    messages = "Login exitoso"
                });
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
