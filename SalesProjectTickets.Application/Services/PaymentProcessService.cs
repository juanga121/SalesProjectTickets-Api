using AutoMapper;
using SalesProjectTickets.Application.DTOs;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Services
{
    public class PaymentProcessService(IRepoPaymentProcess repoPaymentProcess, IMapper mapper) : IServicePaymentProcess
    {
        private readonly IRepoPaymentProcess _repoPaymentProcess = repoPaymentProcess;
        private readonly IMapper _mapper = mapper;
        public async Task AddRetention(PurchaseHistory purchaseHistory)
        {
            var tickets = await _repoPaymentProcess.GetTicketsById(purchaseHistory.PaymentsId);

            if (tickets.Quantity < purchaseHistory.QuantityHistory)
            {
                throw new Exception("No hay disponibilidad para la cantidad de tickets que solicita");
            }

            tickets.Quantity -= purchaseHistory.QuantityHistory;
            await _repoPaymentProcess.UpdateTickets(tickets);

            purchaseHistory.TotalToPay = tickets.Price!.Value * purchaseHistory.QuantityHistory;

            purchaseHistory.PurchaseStatus = PurchaseStatus.Retencion;
            purchaseHistory.Creation_date = DateTime.Now;
            await _repoPaymentProcess.AddRetention(purchaseHistory);
        }

        public async Task<PurchaseHistory> GetPurchaseById(Guid id)
        {
            return await _repoPaymentProcess.GetPurchaseById(id);
        }

        public async Task<List<PaymentProcessDTO>> GetPurchaseHistoryByAdmin()
        {
            var purchaseHistory = await _repoPaymentProcess.GetPurchaseHistoryByAdmin();
            return _mapper.Map<List<PaymentProcessDTO>>(purchaseHistory);
        }

        public async Task<List<PaymentProcessDTO>> GetPurchaseHistoryByUser(Guid id)
        {
            var purchaseHistory = await _repoPaymentProcess.GetPurchaseHistoryByUser(id);
            return _mapper.Map<List<PaymentProcessDTO>>(purchaseHistory);
        }

        public async Task<Tickets> GetTicketsById(Guid id)
        {
            return await _repoPaymentProcess.GetTicketsById(id);
        }

        public async Task UpdatePaymentProcess(PurchaseHistory purchaseHistory)
        {
            await _repoPaymentProcess.UpdatePaymentProcess(purchaseHistory);
        }

        public async Task UpdateTickets(Tickets tickets)
        {
            await _repoPaymentProcess.UpdateTickets(tickets);
        }
    }
}
