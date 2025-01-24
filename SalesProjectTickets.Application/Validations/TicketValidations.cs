using FluentValidation;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;


namespace SalesProjectTickets.Application.Validations
{
    public class TicketValidations : AbstractValidator<Tickets>
    {
        public TicketValidations()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_NAME)
                .Length(3, 50).WithMessage(MessagesError.MESSAGES_LENGTH_NAME);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_DESCRIPTION)
                .Length(5, 255).WithMessage(MessagesError.MESSAGES_LENGTH_DESCRIPTION);
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_QUANTITY)
                .GreaterThan(0).WithMessage(MessagesError.MESSAGES_GREATER_THAN_ZERO);
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_PRICE)
                .GreaterThan(0).WithMessage(MessagesError.MESSAGES_GREATER_THAN_ZERO);
            RuleFor(x => x.Event_date)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(MessagesError.MESSAGES_GREATER_THAN_DATE);
            RuleFor(x => x.Event_location)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_LOCATION)
                .Length(1, 255).WithMessage(MessagesError.MESSAGES_LENGTH_LOCATION);
        }
    }
}
