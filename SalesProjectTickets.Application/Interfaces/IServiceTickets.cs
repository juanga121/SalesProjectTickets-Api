﻿using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Interfaces
{
    public interface IServiceTickets<TEntity>
    : IAddTickets<TEntity>, IListTickets<TEntity>, IEditTickets<TEntity>, IDelete
    {
        public Task ChangeState();
    }
}
