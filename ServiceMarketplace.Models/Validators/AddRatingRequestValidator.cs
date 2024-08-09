using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class AddRatingRequestValidator : AbstractValidator<AddRatingRequestModel>
{
    public AddRatingRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Value)
            .InclusiveBetween(1, 10)
            .WithMessage(Messages.ValueRange);

        RuleFor(x => x.Comment)
            .Length(10, 600)
            .When(x => x.Comment is not null)
            .WithMessage(Messages.FieldLength);
    }
}
