using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;

namespace SalesProjectDomain.Tests
{
    public class UsersTests
    {
        private readonly Mock<IRepoUsers<Users, Guid, Permissions>> _mockRepo;
        private readonly Mock<IValidator<Users>> _mockValidator;
        private readonly UsersServices _userServices;

        public UsersTests()
        {
            _mockRepo = new Mock<IRepoUsers<Users, Guid, Permissions>>();
            _mockValidator = new Mock<IValidator<Users>>();
            _userServices = new UsersServices(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Add_Exception_Validation()
        {
            var invalidInfo = new Users
            {
                Id = Guid.NewGuid(),
                Identity_type = Identity.CedulaCiudadania,
                Document_identity = 123456789,
                Name = "Juan Pablo 2",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#",
                Permissions = Permissions.Administrador,
                Creation_date = DateOnly.FromDateTime(DateTime.Now)
            };

            var validationsFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new("Name", "El nombre solo puede contener letras")
            };

            _mockValidator.Setup(x => x.Validate(invalidInfo)).Returns(new FluentValidation.Results.ValidationResult(validationsFailures));

            var exception = await Assert.ThrowsAsync<PersonalExceptions>(async () => await _userServices.Add(invalidInfo));

            Assert.Contains("El nombre solo puede contener letras", exception.Message);
        }

        [Fact]
        public async Task Add_Exception_ExistingUser()
        {
            var existingUser = new Users
            {
                Identity_type = Identity.CedulaCiudadania,
                Document_identity = 1234567891,
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#",
            };

            _mockValidator.Setup(x => x.Validate(existingUser)).Returns(new ValidationResult());

            _mockRepo.Setup(x => x.UserExisting(existingUser)).ReturnsAsync(existingUser);

            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _userServices.Add(existingUser));

            Assert.Equal("El usuario ya existe, verifique su correo y documento", exception.Message);
        }

        [Fact]
        public async Task Add_Users_Permissions_Admin()
        {
            var user = new Users
            {
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#",
                Permissions = Permissions.SuperUsuario
            };

            var userPermissions = new Users
            {
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#",
                Permissions = Permissions.SuperUsuario
            };

            _mockValidator.Setup(x => x.Validate(user)).Returns(new ValidationResult());

            _mockRepo.Setup(x => x.UserExisting(user)).ReturnsAsync((Users)null!);

            _mockRepo.Setup(x => x.ProviderToken(user.Permissions)).ReturnsAsync(userPermissions);

            var result = await _userServices.Add(user);

            Assert.NotNull(result);
            Assert.Equal(user.Permissions, result.Permissions);
        }
        [Fact]
        public async Task Add_Users_Permissions_Consu()
        {
            var user = new Users
            {
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#"
            };

            _mockValidator.Setup(x => x.Validate(user)).Returns(new ValidationResult());

            _mockRepo.Setup(x => x.UserExisting(user)).ReturnsAsync((Users)null!);

            _mockRepo.Setup(x => x.ProviderToken(user.Permissions)).ReturnsAsync(user);

            var result = await _userServices.Add(user);

            Assert.NotNull(result);
            Assert.Equal(user.Permissions, result.Permissions);
        }
    }
}