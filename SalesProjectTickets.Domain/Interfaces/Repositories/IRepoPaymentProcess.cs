using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces.Repositories
{
    public interface IRepoPaymentProcess
    {
        public Task AddRetention(PurchaseHistory purchaseHistory);
        public Task<Tickets> GetTicketsById(Guid id);
        public Task UpdateTickets(Tickets tickets);
        public Task UpdatePaymentProcess(PurchaseHistory purchaseHistory);
        public Task<PurchaseHistory> GetPurchaseById(Guid id);
    }
}
