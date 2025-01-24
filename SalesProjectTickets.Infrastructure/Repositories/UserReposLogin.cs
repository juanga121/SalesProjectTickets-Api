using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;

namespace SalesProjectTickets.Infrastructure.Repositories
{
    public class UserReposLogin(ContextsDaBa contextsDaBa) : IRepoLogin<LoginUsers>
    {
        private readonly ContextsDaBa _context = contextsDaBa;

        public async Task<LoginUsers?> Login(LoginUsers entity)
        {
            await Task.CompletedTask;
            return entity;
        }

        public async Task<LoginUsers?> Verify(LoginUsers entity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == entity.Email && user.Password == entity.Password);
            if (user == null)
            {
                return null;
            }

            return new LoginUsers
            {
                Email = user.Email,
                Password = user.Password,
                Permissions = user.Permission
            };
        }
    }
}
