using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Infrastructure.Contexts
{
    public class ContextsDaBa(DbContextOptions<ContextsDaBa> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }

        public DbSet<Tickets> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tickets>().HasKey(t => t.Id_ticket);

            modelBuilder.Entity<Tickets>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18, 2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
