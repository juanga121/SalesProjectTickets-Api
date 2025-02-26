using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces.Repositories
{
    public interface IRepoUsers<TEntity>
    : IAdd<TEntity>, IList<TEntity>
    {
    }
}
