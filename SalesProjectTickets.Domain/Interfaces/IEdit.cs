using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface IEdit<in TEntity>
    {
        public Task Edit(TEntity entity);
    }
}
