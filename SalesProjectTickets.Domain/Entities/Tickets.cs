using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class Tickets
    {
        public Guid Id_ticket { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateOnly Event_date { get; set; }
        public required string Event_location { get; set; }
        public TimeOnly Event_time { get; set; }
        public DateOnly Creation_date { get; set; }
        public State State { get; set; }

        public static Tickets Create(string name, string description, int quantity, decimal price, string event_location, State state)
        {
            return new Tickets
            {
                Id_ticket = Guid.NewGuid(),
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = event_location,
                Creation_date = DateOnly.FromDateTime(DateTime.Now),
                State = state
            };
        }
    }
}
