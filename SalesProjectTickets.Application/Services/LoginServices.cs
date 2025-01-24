using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Services
{
    public class LoginServices(IRepoLogin<LoginUsers> _repoLogin) : IServiceLogin<LoginUsers>
    {
        private readonly IRepoLogin<LoginUsers> repoLogin = _repoLogin;

        public async Task<LoginUsers?> Login(LoginUsers entity)
        {
            var userExisting = await repoLogin.Verify(entity);
            return userExisting ?? throw new Exception(MessagesError.MESSAGES_VERIFY_USER);
        }

        public async Task<LoginUsers?> Verify(LoginUsers loginUsers)
        {
            return await repoLogin.Verify(loginUsers);
        }
    }
}
