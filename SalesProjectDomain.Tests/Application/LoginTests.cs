using AutoMapper;
using FluentValidation.Results;
using Moq;
using SalesProjectTickets.Application.DTOs;
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

namespace SalesProjectApplication.Tests.Application
{
    public class LoginTests
    {
        private readonly Mock<IRepoLogin<LoginUsers>> _mockRepo;
        private readonly LoginServices _loginServices;
        private readonly Mock<IMapper> _mapper;

        public LoginTests()
        {
            _mapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepoLogin<LoginUsers>>();
            _loginServices = new LoginServices(_mockRepo.Object, _mapper.Object);
        }

        [Fact]
        public async Task Login()
        {
            var login = new LoginDTO
            {
                Email = "juanpaj542@gmail.com",
                Password = "JuanP12#"
            };

            var loginEntity = new LoginUsers
            {
                Email = login.Email,
                Password = login.Password
            };

            _mapper.Setup(x => x.Map<LoginUsers>(It.IsAny<LoginDTO>())).Returns(loginEntity);
            _mapper.Setup(x => x.Map<LoginDTO>(It.IsAny<LoginUsers>())).Returns(login);

            _mockRepo.Setup(x => x.Verify(It.IsAny<LoginUsers>())).ReturnsAsync(loginEntity);

            var result = await _loginServices.Login(login);

            Assert.NotNull(result);
            Assert.Equal(login.Email, result.Email);
            Assert.Equal(login.Password, result.Password);
        }

        [Fact]
        public async Task Login_failed()
        {
            var login = new LoginDTO
            {
                Email = "pedro@gmail.com",
                Password = "Juana12#"
            };

            _mockRepo.Setup(x => x.Verify(It.IsAny<LoginUsers>())).ReturnsAsync((LoginUsers?)null);

            var result = await Assert.ThrowsAsync<ValidationException>(async () => await _loginServices.Login(login));

            Assert.Equal("Usuario no enconrado o credenciales malas", result.Message);
        }

        [Fact]
        public async Task Verify()
        {
            var login = new LoginDTO
            {
                Email = "pedro@gmail.com",
                Password = "Juana12#"
            };

            var loginEntity = new LoginUsers
            {
                Email = login.Email,
                Password = login.Password
            };

            _mapper.Setup(x => x.Map<LoginUsers>(It.IsAny<LoginDTO>())).Returns(loginEntity);
            _mapper.Setup(x => x.Map<LoginDTO>(It.IsAny<LoginUsers>())).Returns(login);

            _mockRepo.Setup(x => x.Verify(It.IsAny<LoginUsers>())).ReturnsAsync(loginEntity);

            var result = await _loginServices.Verify(login);

            Assert.NotNull(result);
        }
    }
}
