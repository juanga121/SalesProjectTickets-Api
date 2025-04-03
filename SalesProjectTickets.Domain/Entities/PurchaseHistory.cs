using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class PurchaseHistory
    {
        public Guid Id { get; set; }
        public DateTime Creation_date { get; set; }
        [ForeignKey("Tickets")]
        public Guid PaymentsId { get; set; }
        public virtual Tickets? Tickets { get; set; }

        [ForeignKey("Users")]
        public Guid PaymentsUsersId { get; set; }
        public virtual Users? Users { get; set; }

        public int QuantityHistory { get; set; }
        public decimal TotalToPay { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }

        public static PurchaseHistory Create(Guid paymentsId, Guid paymentsUsersId, int quantityHistory, decimal totalToPay, PurchaseStatus purchaseStatus)
        {
            return new PurchaseHistory
            {
                Id = Guid.NewGuid(),
                Creation_date = DateTime.Now,
                PaymentsId = paymentsId,
                PaymentsUsersId = paymentsUsersId,
                QuantityHistory = quantityHistory,
                TotalToPay = totalToPay,
                PurchaseStatus = purchaseStatus
            };
        }
    }
}
