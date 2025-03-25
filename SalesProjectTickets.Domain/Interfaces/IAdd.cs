using Microsoft.AspNetCore.Http;
using SalesProjectTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Interfaces
{
    public interface IAdd<TEntity>
    {
       public Task<TEntity> Add(TEntity entity);
    }

    public interface IAddTickets<TEntity>
    {
        public Task<TEntity> Add(TEntity entity, IFormFile? formFile);
    }
}
