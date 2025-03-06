using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces.Repositories
{
    public interface IRepoTickets<TEntity> : IAddTickets<TEntity>, IListTickets<TEntity>, IEditTickets<TEntity>, IDelete
    {
    }
}
