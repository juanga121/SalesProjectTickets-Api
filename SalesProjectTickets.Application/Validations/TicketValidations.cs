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
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Length(3, 50).WithMessage(MessagesCentral.MESSAGES_LENGTH_FIELDS);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Length(1, 100).WithMessage(MessagesCentral.MESSAGES_LENGTH);
            RuleFor(x => x.Quantity)
                .NotNull().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .GreaterThan(0).WithMessage(MessagesCentral.MESSAGES_GREATER_THAN_ZERO);
            RuleFor(x => x.Price)
                .NotNull().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .GreaterThan(0).WithMessage(MessagesCentral.MESSAGES_GREATER_THAN_ZERO);
            RuleFor(x => x.Event_date)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(MessagesCentral.MESSAGES_GREATER_THAN_DATE);
            RuleFor(x => x.Event_location)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Length(1, 100).WithMessage(MessagesCentral.MESSAGES_LENGTH);
            RuleFor(x => x.Event_time)
                .NotNull().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS);
        }
    }
}
