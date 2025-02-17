using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface IDelete<in TEntityID>
    {
        public Task Delete(TEntityID entityID);
    }
}
