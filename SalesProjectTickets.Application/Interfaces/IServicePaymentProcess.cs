﻿using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Interfaces
{
    public interface IServicePaymentProcess
    {
        public Task AddRetention(PurchaseHistory purchaseHistory);
        public Task<Tickets> GetTicketsById(Guid id);
        public Task UpdateTickets(Tickets tickets);
    }
}
