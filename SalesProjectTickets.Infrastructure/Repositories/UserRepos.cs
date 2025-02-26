using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;
using System.ComponentModel.DataAnnotations;

namespace SalesProjectTickets.Infrastructure.Repositories
{
    public class UserRepos(ContextsDaBa context, IHttpContextAccessor httpContextAccessor) : IRepoUsers<Users>
    {
        private readonly ContextsDaBa _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Users> Add(Users entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Users>> ListAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> ProviderToken(Permissions entityPermi)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                var permissionClaim = user.Claims.FirstOrDefault(claim => claim.Type == "Permissions");

                if (permissionClaim != null && Enum.TryParse(permissionClaim.Value, out Permissions parsedPermissions))
                {
                    return await Task.FromResult(new Users
                    {
                        Name = "",
                        Last_name = "",
                        Email = "",
                        Password = "",
                        Permissions = parsedPermissions
                    });
                }
            }
            return await Task.FromResult<Users?>(null);
        }

        public async Task<Users> SelectionById(Guid entityID)
        {
            Users? user = await _context.Users.FirstOrDefaultAsync(user => user.Id == entityID);
            return user ?? throw new ValidationException("No se encontro el usuario");
        }

        public async Task<Users?> UserExisting(Users entity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == entity.Email || user.Document_identity == entity.Document_identity);
            return user;
        }
    }
}