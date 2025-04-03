using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Infrastructure.Repositories;

namespace SalesProjectTickets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsProcessController(ContextsDaBa contextsDaBa, IMapper mapper) : ControllerBase
    {
        private readonly ContextsDaBa _contextsDaBa = contextsDaBa;
        private readonly IMapper _mapper = mapper;

        PaymentProcessService PaymentProcessService()
        {
            PaymentProcessRepo paymentProcessRepo = new(_contextsDaBa);
            PaymentProcessService paymentProcessService = new(paymentProcessRepo, _mapper);
            return paymentProcessService;
        }

        [HttpPost]
        [Route("AddRetention")]
        [Authorize(Policy = "AdminAndConsu")]
        public async Task<IActionResult> AddRetention(PurchaseHistory purchaseHistory)
        {
            try
            {
                var service = PaymentProcessService();
                await service.AddRetention(purchaseHistory);
                return Ok(new { message = "Retención agregada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminAndConsu")]
        public async Task<IActionResult> UpdatePayment(Guid id, PurchaseHistory purchaseHistory)
        {
            try
            {
                var service = PaymentProcessService();
                purchaseHistory.Id = id;
                await service.UpdatePaymentProcess(purchaseHistory);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndConsu")]
        public async Task<IActionResult> GetPurchaseById(Guid id)
        {
            var service = PaymentProcessService();
            return Ok(await service.GetPurchaseById(id));
        }

        [HttpGet]
        [Route("GetPurchaseHistoryByAdmin")]
        public async Task<IActionResult> GetPurchaseHistoryByAdmin()
        {
            var service = PaymentProcessService();
            return Ok(await service.GetPurchaseHistoryByAdmin());
        }
    }
}
