using FluentValidation;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class ManageBusinessHoursRequestValidator : AbstractValidator<ManageBusinessHoursRequestModel>
{
    public ManageBusinessHoursRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty()
            .WithMessage(Messages.RequiredField);

        RuleFor(x => x.DayOfWeek)
            .IsInEnum()
            .WithMessage(Messages.InvalidDay);

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage(Messages.InvalidStartTime);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage(Messages.InvalidEndTime);
    }
}
