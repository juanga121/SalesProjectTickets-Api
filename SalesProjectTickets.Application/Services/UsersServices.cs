using FluentValidation;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;


namespace SalesProjectTickets.Application.Services
{
    public class UsersServices(IRepoUsers<Users> _repoUser, IValidator<Users> validator) : IServiceUsers<Users>
    {
        private readonly IRepoUsers<Users> reposBase = _repoUser;
        private readonly IValidator<Users> validations = validator;

        public async Task<Users> Add(Users entity)
        {
            var resultValidation = validations.Validate(entity);

            if (!resultValidation.IsValid)
            {
                var errors = resultValidation.Errors
                    .Select(error => new ValidationsError(error.PropertyName, error.ErrorMessage))
                    .ToList();

                throw new PersonalException(errors);
            }

            entity.Creation_date = DateOnly.FromDateTime(DateTime.Now);

            var userExisting = await reposBase.UserExisting(entity);
            if (userExisting != null)
            {
                throw new ValidationException(MessagesError.MESSAGES_EXISTING_USER);
            }

            var userPermissions = await reposBase.ProviderToken(entity.Permissions);

            if (userPermissions?.Permissions == Permissions.SuperUsuario)
            {
                entity.Permissions = Permissions.Administrador;
            }
            else
            {
                entity.Permissions = Permissions.Consumidor;
            }

            await reposBase.Add(entity);
            return entity;
        }

        public async Task<List<Users>> ListAll()
        {
            return await reposBase.ListAll();
        }

        public async Task<Users?> ProviderToken(Permissions entityPermi)
        {
            return await reposBase.ProviderToken(entityPermi);
        }

        public async Task<Users> SelectionById(Guid entityID)
        {
            return await reposBase.SelectionById(entityID);
        }

        public async Task<Users?> UserExisting(Users users)
        {
            return await reposBase.UserExisting(users);
        }
    }
}
