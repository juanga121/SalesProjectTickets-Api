using AutoMapper;
using Moq;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectApplication.Tests.Application
{
    public class PaymentProcessTests
    {
        private readonly Mock<IRepoPaymentProcess> _repoPaymentProcess;
        private readonly Mock<IMapper> _mapper;
        private readonly PaymentProcessService _paymentProcessService;

        public PaymentProcessTests()
        {
            _repoPaymentProcess = new Mock<IRepoPaymentProcess>();
            _mapper = new Mock<IMapper>();
            _paymentProcessService = new PaymentProcessService(_repoPaymentProcess.Object, _mapper.Object);
        }
        [Fact]
        public async Task AddRetention_exception()
        {
            var purchaseHistory = new PurchaseHistory
            {
                PaymentsId = Guid.NewGuid(),
                QuantityHistory = 5
            };

            var tickets = new Tickets
            {
                Quantity = 3,
                Price = 10
            };

            _repoPaymentProcess.Setup(x => x.GetTicketsById(purchaseHistory.PaymentsId)).ReturnsAsync(tickets);

            var exception = await Assert.ThrowsAsync<Exception>(() => _paymentProcessService.AddRetention(purchaseHistory));

            Assert.Equal("No hay disponibilidad para la cantidad de tickets que solicita", exception.Message);
        }

        [Fact]
        public async Task Add_retention_succesfully()
        {
            var purchaseHistory = new PurchaseHistory
            {
                PaymentsId = Guid.NewGuid(),
                QuantityHistory = 5
            };

            var tickets = new Tickets
            {
                Quantity = 50,
                Price = 10
            };

            _repoPaymentProcess.Setup(x => x.GetTicketsById(purchaseHistory.PaymentsId)).ReturnsAsync(tickets);

            await _paymentProcessService.AddRetention(purchaseHistory);

            Assert.NotNull(purchaseHistory);
        }
    }
}
