using AutoMapper;
using SalesProjectTickets.Application.DTOs;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SalesProjectTickets.Application.Services
{
    public class LoginServices(IRepoLogin<LoginUsers> _repoLogin, IMapper mapper) : IServiceLogin<LoginDTO>
    {
        private readonly IRepoLogin<LoginUsers> repoLogin = _repoLogin;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginDTO?> Login(LoginDTO entityDTO)
        {
            var entityLogin = _mapper.Map<LoginUsers>(entityDTO);

            var userExisting = await repoLogin.Verify(entityLogin);

            return userExisting == null
                ? throw new ValidationException(MessagesCentral.MESSAGES_VERIFY_USER)
                : _mapper.Map<LoginDTO>(userExisting);
        }

        public async Task<LoginDTO?> Verify(LoginDTO loginUsersDTO)
        {
            var loginUsers = _mapper.Map<LoginUsers>(loginUsersDTO);
            var userExisting = await repoLogin.Verify(loginUsers);
            return userExisting != null ? _mapper.Map<LoginDTO>(userExisting) : null;
        }
    }
}
