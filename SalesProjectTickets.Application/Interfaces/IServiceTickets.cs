using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Interfaces
{
    public interface IServiceTickets<TEntity, TEntityID, TEntityPermi>
    : IAdd<TEntity>, IListTickets<TEntity, TEntityPermi>, IEdit<TEntity>, IDelete<TEntityID>
    {
    }
}
