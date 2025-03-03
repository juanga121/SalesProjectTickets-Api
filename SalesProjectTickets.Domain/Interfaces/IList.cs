using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface IList<TEntity>
    {
        public Task<List<TEntity>> ListAll();

        public Task<TEntity> SelectionById(Guid entityID);

        public Task<TEntity?> ProviderToken(Permissions entityPermi);

        public Task<TEntity?> UserExisting(TEntity entity);
    }
    public interface IListTickets<TEntity>
    {
        public Task<List<TEntity>> ListAllTickets();
        public Task<TEntity> SelectionById(Guid entity);

        public Task<bool> ListByPermissions(Permissions entityPermi);
    }
}
