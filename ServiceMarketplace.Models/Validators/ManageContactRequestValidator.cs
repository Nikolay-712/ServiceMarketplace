using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class ManageContactRequestValidator : AbstractValidator<ManageContactRequestModel>
{
    public ManageContactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Name)
            .Length(10, 150)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.PhoneNumber)
            .Must(x => ValidatorHelper.BulgarianMobileNumberValidation(x))
            .WithMessage(Messages.InvalidPhnoneNumber);

        RuleFor(x => x.LocationUrl)
            .Must(x => ValidatorHelper.UrlValidation(x))
            .When(x => x.LocationUrl is not null)
            .WithMessage(Messages.InvalidUrl);
    }
}
