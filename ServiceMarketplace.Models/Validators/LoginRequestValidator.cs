using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestModel>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Email)
            .Must(x => ValidatorHelper.ValidateEmail(x))
            .WithMessage(Messages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);
    }
}
