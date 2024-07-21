using FluentValidation;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Models.Validators;

public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequestModel>
{
    public CreateServiceRequestValidator()
    {
    }
}
