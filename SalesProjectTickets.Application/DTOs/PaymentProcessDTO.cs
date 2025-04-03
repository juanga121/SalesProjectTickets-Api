using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.DTOs
{
    public class PaymentProcessDTO
    {
        public Guid Id { get; set; }
        public DateTime Creation_date { get; set; }
        public int QuantityHistory { get; set; }
        public decimal TotalToPay { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }
        public string? NameTicket { get; set; }
        public string? NameUser { get; set; }
        public string? EmailUser { get; set; }
    }
}
