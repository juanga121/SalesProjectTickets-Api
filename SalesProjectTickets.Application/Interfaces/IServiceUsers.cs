using SalesProjectTickets.Domain.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Interfaces
{
    public interface IServiceUsers<TEntity>
        : IAdd<TEntity>, Domain.Interfaces.IList<TEntity>
    {
    }
}
