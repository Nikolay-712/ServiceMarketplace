using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class ManageCityRequestValidator : AbstractValidator<ManageCityRequestModel>
{
    public ManageCityRequestValidator()
    {
        RuleFor(x => x.NameBg)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.NameBg)
            .Length(5, 100)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.NameEn)
           .NotEmpty()
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.NameEn)
            .Length(5, 100)
            .WithMessage(Messages.FieldLength);
    }
}

