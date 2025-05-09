﻿using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
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

        public async Task<PurchaseHistory> GetPurchaseById(Guid id)
        {
            var tickets = await _context.PurchaseHistory
                .Where(payment => payment.PaymentsUsersId == id && payment.PurchaseStatus == PurchaseStatus.Retencion)
                .OrderByDescending(payment => payment.Creation_date)
                .FirstOrDefaultAsync();

            return tickets ?? throw new Exception("No se encontro el pago");
        }

        public async Task<List<PurchaseHistory>> GetPurchaseHistoryByAdmin()
        {
            return await _context.PurchaseHistory
                .Include(p => p.Users)
                .Include(p => p.Tickets)
                .OrderByDescending(p => p.Creation_date)
                .ToListAsync();
        }

        public async Task<List<PurchaseHistory>> GetPurchaseHistoryByUser(Guid id)
        {
            return await _context.PurchaseHistory
                .Include(p => p.Users)
                .Include(p => p.Tickets)
                .Where(p => p.PaymentsUsersId == id)
                .OrderByDescending(p => p.Creation_date)
                .ToListAsync();
        }

        public async Task<Tickets> GetTicketsById(Guid id)
        {
            var tickets = await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == id);
            return tickets ?? throw new Exception("No se encontro el ticket");
        }

        public async Task<List<PurchaseHistory>> RetentionList()
        {
            return await _context.PurchaseHistory.ToListAsync();
        }

        public async Task UpdatePaymentProcess(PurchaseHistory purchaseHistory)
        {
            var purchase = await _context.PurchaseHistory.FirstOrDefaultAsync(ticket => ticket.Id == purchaseHistory.Id);

            if (purchase != null)
            {
                purchase.PurchaseStatus = purchaseHistory.PurchaseStatus;
                _context.SaveChanges();
            }
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
