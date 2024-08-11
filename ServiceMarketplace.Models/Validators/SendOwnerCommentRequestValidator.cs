using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class SendOwnerCommentRequestValidator : AbstractValidator<SendOwnerCommentRequestModel>
{
    public SendOwnerCommentRequestValidator()
    {
        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.Comment)
            .Length(10, 500)
            .WithMessage(Messages.FieldLength);
    }
}
