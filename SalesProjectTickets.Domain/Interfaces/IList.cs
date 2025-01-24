using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface IList<TEntity, TEntityID, TEntityPermi>
    {
        public Task<List<TEntity>> ListAll();

        public Task<TEntity> SelectionById(TEntityID entityID);

        public Task<TEntity?> ProviderToken(TEntityPermi entity);

        public Task<TEntity?> UserExisting(TEntity entity);
    }
    public interface IListTickets<TEntity, TEntityPermi>
    {
        public Task<List<TEntity>> ListAllTickets();

        public Task<bool> ListByPermissions(TEntityPermi entity);
    }
}
