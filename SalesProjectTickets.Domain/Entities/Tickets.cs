using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class Tickets : EntityBase
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateOnly Event_date { get; set; }
        public required string Event_location { get; set; }
        public TimeOnly Event_time { get; set; }
        public State State { get; set; }

        public static Tickets Create(string name, string description, int quantity, decimal price, string event_location, State state)
        {
            return new Tickets
            {
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = event_location,
                State = state
            };
        }
    }
}
