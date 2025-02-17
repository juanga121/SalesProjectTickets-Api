using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public Permissions Permissions { get; set; }
        public DateOnly Creation_date { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
            Creation_date = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
