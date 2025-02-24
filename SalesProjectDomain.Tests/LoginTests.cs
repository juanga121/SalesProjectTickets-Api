using FluentValidation.Results;
using Moq;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SalesProjectApplication.Tests
{
    public class LoginTests
    {
        private readonly Mock<IRepoLogin<LoginUsers>> _mockRepo;
        private readonly LoginServices _loginServices;

        public LoginTests()
        {
            _mockRepo = new Mock<IRepoLogin<LoginUsers>>();
            _loginServices = new LoginServices(_mockRepo.Object);
        }

        [Fact]
        public async Task Login()
        {
            var login = new LoginUsers
            {
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#"
            };

            _mockRepo.Setup(x => x.Verify(login)).ReturnsAsync(login);

            var result = await _loginServices.Login(login);

            Assert.NotNull(result);
            Assert.True(result == login);
        }

        [Fact]
        public async Task Login_failed()
        {
            var login = new LoginUsers
            {
                Email = "pedro@gmail.com",
                Password = "Juana12#"
            };

            _mockRepo.Setup(x => x.Verify(login)).ReturnsAsync((LoginUsers?)null);

            var result = await Assert.ThrowsAsync<ValidationException>(async () => await _loginServices.Login(login));

            Assert.Equal("Usuario no enconrado o credenciales malas" ,result.Message);
        }

        [Fact]
        public async Task Verify()
        {
            var login = new LoginUsers
            {
                Email = "pedro@gmail.com",
                Password = "Juana12#"
            };

            _mockRepo.Setup(x => x.Verify(login)).ReturnsAsync(login);

            var result = await _loginServices.Verify(login);

            Assert.NotNull(result);
        }
    }
}
