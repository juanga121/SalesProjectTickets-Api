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

namespace SalesProjectTickets.Tests.TestTickets
{
    public class TestsTickets
    {
        private readonly Mock<IRepoTickets<Tickets, Guid, Permissions>> _mockRepo;
        private readonly Mock<IValidator<Tickets>> _mockValidator;
        private readonly TicketsServices _ticketsServices;

        public TestsTickets()
        {
            _mockRepo = new Mock<IRepoTickets<Tickets, Guid, Permissions>>();
            _mockValidator = new Mock<IValidator<Tickets>>();
            _ticketsServices = new TicketsServices(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Successfully_add_fluentValidationTickets() // prueba para saber la validacion de campos
        {
            var tickets = new Tickets
            {
                Name = "",
                Description = "Concierto de rock",
                Quantity = 100,
                Price = 50000,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = "Estadio",
                Event_time = TimeOnly.FromDateTime(DateTime.Now),
                Creation_date = DateOnly.FromDateTime(DateTime.Now),
                State = State.Disponible
            };

            // Configuracion para que la validacion falle
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<Tickets>()))
                .Returns(new FluentValidation.Results.ValidationResult(
                    [new FluentValidation.Results.ValidationFailure("Name", "El nombre es requerido")]));

            // Verificar que la excepcion sea lanzada
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _ticketsServices.Add(tickets));
            Assert.Equal("El nombre es requerido", exception.Message);
        }
    }
}
