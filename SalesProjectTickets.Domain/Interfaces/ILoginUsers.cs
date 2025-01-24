using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface ILoginUsers<TEntity>
    {
        public Task<TEntity?> Login(TEntity entity);

        public Task<TEntity?> Verify(TEntity entity);
    }
}
