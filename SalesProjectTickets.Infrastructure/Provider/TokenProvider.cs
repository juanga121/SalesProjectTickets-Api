using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SalesProjectTickets.Domain.Entities;
using System.Security.Claims;
using System.Text;

namespace SalesProjectTickets.Infrastructure.Provider
{
    public class TokenProvider(IConfiguration configuration)
    {
        public string GenerateToken(LoginUsers loginUsers)
        {
            string? SecretKey = configuration["Jwt:SecretKey"];
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Email, loginUsers.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Permissions", loginUsers.Permissions.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = Credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var TokenHandler = new JsonWebTokenHandler();

            string token = TokenHandler.CreateToken(TokenDescriptor);
            return token;
        }
    }
}
