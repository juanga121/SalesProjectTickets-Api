using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;

namespace SalesProjectApplication.Tests.Application
{
    public class UsersTests
    {
        private readonly Mock<IRepoUsers<Users>> _mockRepo;
        private readonly Mock<IValidator<Users>> _mockValidator;
        private readonly UsersServices _userServices;

        public UsersTests()
        {
            _mockRepo = new Mock<IRepoUsers<Users>>();
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
                Document_identity = 1234567892,
                Name = "Juan Pablo 2",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#",
                Permissions = Permissions.Administrador,
                Creation_date = DateOnly.FromDateTime(DateTime.Now)
            };

            var validationsFailures = new List<ValidationFailure>
            {
                new("Name", "El nombre solo puede contener letras")
            };

            _mockValidator.Setup(x => x.Validate(invalidInfo)).Returns(new ValidationResult(validationsFailures));

            var exception = await Assert.ThrowsAsync<PersonalException>(async () => await _userServices.Add(invalidInfo));

            Assert.Equal("Error de validación", exception.Message);
            Assert.Contains(exception.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "El nombre solo puede contener letras");
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

        [Fact]
        public async Task ListAll()
        {
            var users = new List<Users>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Identity_type = Identity.CedulaCiudadania,
                    Document_identity = 1234567892,
                    Name = "Juan Pablo",
                    Last_name = "Giraldo Atehortua",
                    Email = "juanpaj542@gmail.com",
                    Password = "JuanP12#",
                    Permissions = Permissions.Administrador,
                    Creation_date = DateOnly.FromDateTime(DateTime.Now)
                },
                new() {
                    Id = Guid.NewGuid(),
                    Identity_type = Identity.CedulaCiudadania,
                    Document_identity = 1234567892,
                    Name = "Juan David",
                    Last_name = "Arazcuta",
                    Email = "arazx2@gmail.com",
                    Password = "Araxcv12#",
                    Permissions = Permissions.Administrador,
                    Creation_date = DateOnly.FromDateTime(DateTime.Now)
                }
            };

            _mockRepo.Setup(x => x.ListAll()).ReturnsAsync(users);

            var result = await _userServices.ListAll();

            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
        }

        [Fact]
        public async Task ProviderToken()
        {
            var user = new Users
            {
                Name = "",
                Last_name = "",
                Email = "",
                Password = "",
                Permissions = Permissions.SuperUsuario
            };

            _mockRepo.Setup(x => x.ProviderToken(user.Permissions)).ReturnsAsync(user);

            var result = await _userServices.ProviderToken(user.Permissions);

            Assert.NotNull(result);
            Assert.Contains("SuperUsuario", result.Permissions.ToString());
        }

        [Fact]
        public async Task UserExisting()
        {
            var user = new Users
            {
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj12@gmail.com",
                Password = "JuanP12#"
            };

            _mockRepo.Setup(x => x.UserExisting(user)).ReturnsAsync(user);

            var result = await _userServices.UserExisting(user);

            Assert.Equal(user, result);
        }
    }
}