using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestModel>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.FullName)
           .NotEmpty()
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.FullName)
            .Length(15, 150)
            .WithMessage(Messages.FieldLength);


        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Email)
            .Must(x => ValidatorHelper.ValidateEmail(x))
            .WithMessage(Messages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.ConfirmPassword)
            .Length(6, 20)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x)
            .Must(x => x.Password.Equals(x.ConfirmPassword)).WithName("Password")
            .When(x => x.Password is not null && x.ConfirmPassword is not null)
            .WithMessage(Messages.PasswordMismatch);
    }
}
