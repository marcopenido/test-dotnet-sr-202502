using System;
using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Domain.Extensions;
using ApplicantTracking.Exceptions;
using FluentValidation;

namespace ApplicantTracking.Application.UseCase.Candidate;

public class CandidateValidator : AbstractValidator<RequestCandidateJson>
{
    public CandidateValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MaximumLength(80).WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);

        RuleFor(c => c.Surname)
            .NotEmpty().WithMessage(ResourceMessagesException.SURNAME_EMPTY)
            .MaximumLength(150).WithMessage(ResourceMessagesException.SURNAME_MAX_LENGTH);

        RuleFor(c => c.Birthdate)
            .Must(b => b != default).WithMessage(ResourceMessagesException.BIRTHDATE_EMPTY)
            .LessThan(DateTime.UtcNow).WithMessage(ResourceMessagesException.BIRTHDATE_INVALID);

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)            
            .MaximumLength(250).WithMessage(ResourceMessagesException.EMAIL_MAX_LENGTH);
        When(c => c.Email.NotEmpty(), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}
