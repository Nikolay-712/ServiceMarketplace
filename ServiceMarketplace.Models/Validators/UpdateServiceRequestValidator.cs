using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequestModel>
{
    public UpdateServiceRequestValidator()
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

        RuleFor(x => x.Cities)
            .Must(x => x.Count > 0)
            .When(x => x.Cities is not null)
            .WithMessage(Messages.RequiredField);
    }
}
