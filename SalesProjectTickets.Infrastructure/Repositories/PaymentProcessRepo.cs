using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Infrastructure.Repositories
{
    public class PaymentProcessRepo(ContextsDaBa contextsDaBa) : IRepoPaymentProcess
    {
        private readonly ContextsDaBa _context = contextsDaBa;

        public async Task AddRetention(PurchaseHistory purchaseHistory)
        {
            await _context.PurchaseHistory.AddAsync(purchaseHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<Tickets> GetTicketsById(Guid id)
        {
            var tickets = await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == id);
            return tickets ?? throw new Exception("No se encontro el ticket");
        }

        public async Task UpdateTickets(Tickets tickets)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == tickets.Id);

            if (ticket != null)
            {
                ticket.Quantity = tickets.Quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
