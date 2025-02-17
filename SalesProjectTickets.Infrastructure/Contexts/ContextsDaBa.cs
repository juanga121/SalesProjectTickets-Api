using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Infrastructure.Contexts
{
    public class ContextsDaBa(DbContextOptions<ContextsDaBa> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<Tickets> Tickets { get; set; }
    }
}
