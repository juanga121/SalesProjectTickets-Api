using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces.Repositories
{
    public interface IRepoTickets<TEntity, TEntityID, TEntityPermi>
    : IAdd<TEntity>, IListTickets<TEntity, TEntityPermi>, IEdit<TEntity>, IDelete<TEntityID>
    {
    }
}
