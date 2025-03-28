using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class LoginUsers
    {
        public Guid Identity_User { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Permissions Permissions { get; set; }
    }
}
