using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Application.Exceptions
{
    public class PersonalException : Exception
    {
        public List<ValidationsError> Errors { get; }

        public PersonalException(List<ValidationsError> validationsErrors) : base("Error de validación")
        {
            Errors = validationsErrors;
        }
    }
}
