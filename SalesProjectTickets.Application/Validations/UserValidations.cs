using FluentValidation;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Application.Validations
{
    public class UserValidations : AbstractValidator<Users>
    {
        public UserValidations()
        {
            RuleFor(x => x.Document_identity)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_DOCUMENT)
                .Must(id => id.ToString().Length == 10).WithMessage(MessagesError.MESSAGES_LENGTH_DOCUMENT);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_NAME)
                .Length(3, 50).WithMessage(MessagesError.MESSAGES_LENGTH_NAME)
                .Matches(ConstantsUser.ONLY_LETTERS).WithMessage(MessagesError.MESSAGES_ONLY_LETTERS_NAME);
            RuleFor(x => x.Last_name)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_LASTNAME)
                .Length(3, 50).WithMessage(MessagesError.MESSAGES_LENGTH_LASTNAME)
                .Matches(ConstantsUser.ONLY_LETTERS).WithMessage(MessagesError.MESSAGES_ONLY_LETTERS_LASTNAME);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_EMAIL)
                .EmailAddress().WithMessage(MessagesError.MESSAGES_INVALID_EMAIL);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(MessagesError.MESSAGES_REQUERID_PASSWORD)
                .Matches(ConstantsUser.PASS_PATTERN);
        }
    }
}
