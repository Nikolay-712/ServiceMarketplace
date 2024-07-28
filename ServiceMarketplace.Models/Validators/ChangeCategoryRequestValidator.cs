using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class ChangeCategoryRequestValidator : AbstractValidator<ChangeCategoryRequestModel>
{
    public ChangeCategoryRequestValidator()
    {
        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleForEach(x => x.Tags)
           .NotEmpty()
           .When(x => x.Tags is not null)
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Tags)
            .Must(x => x.Count > 0)
            .When(x => x.Tags is not null)
            .WithMessage(Messages.RequiredField);
    }
}
