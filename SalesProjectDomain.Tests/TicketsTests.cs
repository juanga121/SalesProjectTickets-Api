using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesProjectTickets.Application.Exceptions;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;

namespace SalesProjectApplication.Tests
{
    public class TicketsTests
    {
        private readonly Mock<IRepoTickets<Tickets>> _mockRepo;
        private readonly Mock<IValidator<Tickets>> _mockValidator;
        private readonly TicketsServices _ticketsServices;

        public TicketsTests()
        {
            _mockRepo = new Mock<IRepoTickets<Tickets>>();
            _mockValidator = new Mock<IValidator<Tickets>>();
            _ticketsServices = new TicketsServices(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Add_Exception_Validation_Tickets()
        {
            var invalidInfo = new Tickets
            {
                Name = "C",
                Description = "Concierto de rock",
                Quantity = 100,
                Price = 50000,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = "Estadio",
                Event_time = TimeOnly.FromDateTime(DateTime.Now),
                State = State.Disponible
            };

            var validationsFailures = new List<ValidationFailure>
            {
                new("Name", "El nombre debe tener entre 3 a 50 caracteres")
            };

            _mockValidator.Setup(x => x.Validate(invalidInfo)).Returns(new ValidationResult(validationsFailures));

            var exception = await Assert.ThrowsAsync<PersonalException>(async () => await _ticketsServices.Add(invalidInfo));

            Assert.Equal("Error de validación", exception.Message);
            Assert.Contains(exception.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "El nombre debe tener entre 3 a 50 caracteres");
        }

        [Fact]
        public async Task Add_Tickest_succesfully() 
        {
            var validInfo = new Tickets
            {
                Name = "Concierto",
                Description = "Concierto de rock",
                Quantity = 100,
                Price = 50000,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = "Estadio",
                Event_time = TimeOnly.FromDateTime(DateTime.Now),
                State = State.Disponible
            };
            _mockValidator.Setup(x => x.Validate(validInfo)).Returns(new ValidationResult());

            var result = await _ticketsServices.Add(validInfo);

            Assert.Equal(validInfo, result);
            Assert.Equal(State.Disponible, result.State);
        }

        [Fact]
        public async Task ListByPermissions_return_true()
        {
            var permiAdmin = Permissions.Administrador;

            _mockRepo.Setup(x => x.ListByPermissions(permiAdmin)).ReturnsAsync(true);

            var result = await _ticketsServices.ListByPermissions(permiAdmin);

            Assert.True(result);
        }

        [Fact]
        public async Task ListByPermissions_return_false()
        {
            var permiAdmin = Permissions.SuperUsuario;

            _mockRepo.Setup(x => x.ListByPermissions(permiAdmin)).ReturnsAsync(false);

            var result = await _ticketsServices.ListByPermissions(permiAdmin);

            Assert.False(result);
        }

        [Fact]
        public async Task ChangeState()
        {
            var allTickets = new List<Tickets>
            {
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Disponible
                },
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Disponible
                }
            };

            _mockRepo.Setup(x => x.ListAllTickets()).ReturnsAsync(allTickets);

            await _ticketsServices.ChangeState();

            Assert.All(allTickets, ticket => Assert.Equal(State.Expirado, ticket.State));
        }

        [Fact]
        public async Task ListAllTickets_Admin()
        {
            var allTickets = new List<Tickets>
            {
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Disponible
                },
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Expirado
                }
            };

            _mockRepo.Setup(x => x.ListAllTickets()).ReturnsAsync(allTickets);

            _mockRepo.Setup(x => x.ListByPermissions(Permissions.Administrador)).ReturnsAsync(true);

            var result = await _ticketsServices.ListAllTickets();

            Assert.Equal(allTickets, result);
        }

        [Fact]
        public async Task ListAllTickets_Consu()
        {
            var allTickets = new List<Tickets>
            {
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Disponible
                },
                new()
                {
                    Name = "Concierto",
                    Description = "Concierto de rock",
                    Quantity = 100,
                    Price = 50000,
                    Event_date = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    Event_location = "Estadio",
                    Event_time = TimeOnly.FromDateTime(DateTime.Now),
                    State = State.Disponible
                }
            };

            _mockRepo.Setup(x => x.ListAllTickets()).ReturnsAsync(allTickets);

            _mockRepo.Setup(x => x.ListByPermissions(Permissions.Administrador)).ReturnsAsync(false);

            var result = await _ticketsServices.ListAllTickets();

            Assert.DoesNotContain(result, ticket => ticket.State == State.Expirado);
            Assert.Contains(result, ticket => ticket.State == State.Disponible);
        }
    }
}
