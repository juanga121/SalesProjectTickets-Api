using SalesProjectTickets.Application.DTOs;
using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Interfaces
{
    public interface IServicePaymentProcess
    {
        public Task<List<PurchaseHistory>> RetentionList();
        public Task AddRetention(PurchaseHistory purchaseHistory);
        public Task<Tickets> GetTicketsById(Guid id);
        public Task UpdateTickets(Tickets tickets);
        public Task UpdatePaymentProcess(PurchaseHistory purchaseHistory);
        public Task<PurchaseHistory> GetPurchaseById(Guid id);
        public Task<List<PaymentProcessDTO>> GetPurchaseHistoryByAdmin();
        public Task<List<PaymentProcessDTO>> GetPurchaseHistoryByUser(Guid id);
        public Task ChangeStatusAfter();
    }
}
