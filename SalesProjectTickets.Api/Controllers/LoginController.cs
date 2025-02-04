using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Infrastructure.Provider;


namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IServiceLogin<LoginUsers> _servicesLogin, TokenProvider tokenProvider) : ControllerBase
    {
        private readonly IServiceLogin<LoginUsers> servicesLogin = _servicesLogin;

        [HttpPost]
        public async Task<IActionResult> Login(LoginUsers loginUsers)
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

                var token = tokenProvider.GenerateToken(users);

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
