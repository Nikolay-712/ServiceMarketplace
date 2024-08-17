using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class AssignUserToRoleRequestValidator : AbstractValidator<AssignUserToRoleRequestModel>
{
    public AssignUserToRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);
    }
}
