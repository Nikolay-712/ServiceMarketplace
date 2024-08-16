using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class MangeRoleRequestValidator : AbstractValidator<ManageRoleRequestModel>
{
    public MangeRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Name)
            .Length(5, 25)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.DescriptionEn)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.DescriptionEn)
            .Length(10, 200)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.DescriptionBg)
           .NotEmpty()
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.DescriptionBg)
            .Length(10, 200)
            .WithMessage(Messages.FieldLength);
    }
}
