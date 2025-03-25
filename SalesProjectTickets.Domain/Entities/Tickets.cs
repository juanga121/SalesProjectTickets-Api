using SalesProjectTickets.Domain.Enums;

namespace SalesProjectTickets.Domain.Entities
{
    public class Tickets : EntityBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateOnly Event_date { get; set; }
        public string? Event_location { get; set; }
        public string? Event_time { get; set; }
        public State State { get; set; }
        public string? ImageUrl { get; set; }

        public static Tickets Create(string name, string description, int quantity, decimal price, string event_location, string event_time, State state)
        {
            return new Tickets
            {
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,
                Event_date = DateOnly.FromDateTime(DateTime.Now),
                Event_location = event_location,
                Event_time = event_time,
                State = state
            };
        }
    }
}
