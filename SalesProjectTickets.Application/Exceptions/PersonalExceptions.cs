using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Application.Exceptions
{
    public class PersonalExceptions : Exception
    {
        public List<ValidationsError> Errors { get; }

        public PersonalExceptions(List<ValidationsError> validationsErrors) : base("Error de validación")
        {
            Errors = validationsErrors;
        }
    }
}
