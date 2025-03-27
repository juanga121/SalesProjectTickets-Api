using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class PurchaseHistory : EntityBase
    {
        [ForeignKey("Tickets")]
        public Guid PaymentsId { get; set; }
        public virtual Tickets? Tickets { get; set; }

        [ForeignKey("Users")]
        public Guid PaymentsUsersId { get; set; }
        public virtual Users? Users { get; set; }

        public int Quantity { get; set; }
        public decimal TotalToPay { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }

    }
}
