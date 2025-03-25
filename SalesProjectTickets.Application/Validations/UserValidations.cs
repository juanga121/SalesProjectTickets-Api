using FluentValidation;
using Microsoft.AspNetCore.Http;
using SalesProjectTickets.Application.Shared;
using SalesProjectTickets.Domain.Entities;

namespace SalesProjectTickets.Application.Validations
{
    public class UserValidations : AbstractValidator<Users>
    {
        public UserValidations()
        {
            RuleFor(x => x.Document_identity)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Must(id => id.ToString().Length == 10).WithMessage(MessagesCentral.MESSAGES_LENGTH_DOCUMENT);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Length(3, 50).WithMessage(MessagesCentral.MESSAGES_LENGTH_FIELDS)
                .Matches(ConstantsUser.ONLY_LETTERS).WithMessage(MessagesCentral.MESSAGES_ONLY_LETTERS);
            RuleFor(x => x.Last_name)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Length(3, 50).WithMessage(MessagesCentral.MESSAGES_LENGTH_FIELDS)
                .Matches(ConstantsUser.ONLY_LETTERS).WithMessage(MessagesCentral.MESSAGES_ONLY_LETTERS);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .EmailAddress().WithMessage(MessagesCentral.MESSAGES_INVALID_EMAIL);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(MessagesCentral.MESSAGES_REQUERID_FIELDS)
                .Matches(ConstantsUser.PASS_PATTERN);
        }
    }
}
