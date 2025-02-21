
namespace SalesProjectTickets.Domain.Entities
{
    public class ValidationsError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }

        public ValidationsError(string propertyName, string errorMessage) 
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
