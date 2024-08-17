using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class RemoveUserFromRoleRequestValidator : AbstractValidator<RemoveUserFromRoleRequestModel>
{
    public RemoveUserFromRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.RoleName)
          .NotEmpty()
          .WithMessage(Messages.RequiredField);
    }
}
