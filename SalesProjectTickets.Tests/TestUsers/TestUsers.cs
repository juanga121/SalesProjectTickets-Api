using FluentValidation;
using Moq;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Tests.TestUsers
{
    public class TestsUsers
    {
        private readonly Mock<IRepoUsers<Users, Guid, Permissions>> _mockRepo;
        private readonly Mock<IValidator<Users>> _mockValidator;
        private readonly UsersServices _userServices;

        public TestsUsers()
        {
            _mockRepo = new Mock<IRepoUsers<Users, Guid, Permissions>>();
            _mockValidator = new Mock<IValidator<Users>>();
            _userServices = new UsersServices(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Successfully_add_fluentValidation() // prueba para saber la validacion de campos
        {
            var userr = new Users
            {
                Identity_type = Identity.CedulaCiudadania,
                Document_identity = 1055750957,
                Name = "",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj@gmail.com",
                Password = "JuanP12#",
                Creation_date = DateOnly.FromDateTime(DateTime.Now),
                Permission = Permissions.SuperUsuario
            };

            // Configuracion para que la validacion falle
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<Users>()))
                .Returns(new FluentValidation.Results.ValidationResult(
                    [new FluentValidation.Results.ValidationFailure("Name", "El nombre es requerido")]));

            // Verificar que la excepcion sea lanzada
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userServices.Add(userr));
            Assert.Equal("El nombre es requerido", exception.Message);
        }

        [Fact]
        public async Task Successfully_add_userExisting() // Prueba para validacion de usuario existente
        {
            var userr = new Users
            {
                Identity_type = Identity.CedulaCiudadania,
                Document_identity = 1055750957,
                Name = "Juan Pablo",
                Last_name = "Giraldo Atehortua",
                Email = "juanpaj@gmail.com",
                Password = "JuanP12#",
                Creation_date = DateOnly.FromDateTime(DateTime.Now),
                Permission = Permissions.SuperUsuario
            };

            _mockValidator
                .Setup(v => v.Validate(It.IsAny<Users>()))
                .Returns(new FluentValidation.Results.ValidationResult()); //Validacion exitosa

            _mockRepo
                .Setup(r => r.UserExisting(It.IsAny<Users>()))
                .ReturnsAsync(new Users
                {
                    Identity_type = Identity.CedulaCiudadania,
                    Document_identity = 1055750957,
                    Name = "Juan Pablo",
                    Last_name = "Giraldo Atehortua",
                    Email = "juanpaj@gmail.com",
                    Password = "JuanP12#",
                    Creation_date = DateOnly.FromDateTime(DateTime.Now),
                    Permission = Permissions.SuperUsuario
                }); // Configuracion para que el repo retorne un usuario existente

            // Verificar que la excepcion sea lanzada
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userServices.Add(userr));
            Assert.Equal("El usuario ya existe, verifique su correo y documento", exception.Message);
        }
    }
}
