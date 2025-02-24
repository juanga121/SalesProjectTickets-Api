using SalesProjectTickets.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Domain.Entities
{
    public class Users : EntityBase
    {
        public Identity Identity_type { get; set; }
        public int Document_identity { get; set; }
        public required string Name { get; set; }
        public required string Last_name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Permissions Permissions { get; set; }
        public static Users Create(Identity identity_type, int document_identity, string name, string last_name, string email, string password)
        {
            return new Users { Identity_type = identity_type, Document_identity = document_identity, Name = name, Last_name = last_name, Email = email, Password = password };
        }
    }
}
