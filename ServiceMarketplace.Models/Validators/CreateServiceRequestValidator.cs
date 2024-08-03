using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequestModel>
{
    public CreateServiceRequestValidator()
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

        RuleFor(x => x.DescriptionBg)
           .NotEmpty()
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.DescriptionBg)
            .Length(15, 500)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.DescriptionEn)
          .NotEmpty()
          .WithMessage(Messages.RequiredField);

        RuleFor(x => x.DescriptionEn)
            .Length(15, 500)
            .WithMessage(Messages.FieldLength);

        RuleFor(x => x.OfferedAtId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.SubCategoryId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleForEach(x => x.Cities)
            .NotEmpty()
            .When(x => x.Cities is not null)
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Cities)
            .Must(x => x.Count > 0)
            .When(x => x.Cities is not null)
            .WithMessage(Messages.RequiredField);

        RuleForEach(x => x.Tags)
           .NotEmpty()
           .When(x => x.Tags is not null)
           .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Tags)
            .Must(x => x.Count > 0)
            .When(x => x.Tags is not null)
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.ContactRequestModel)
            .NotNull()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.ContactRequestModel)
            .SetValidator(x => new ManageContactRequestValidator())
            .When(x => x.ContactRequestModel is not null);
    }
}
