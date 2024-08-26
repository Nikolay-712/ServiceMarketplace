using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Services;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IBusinessHoursService
{
    Task MangeAsync(ManageBusinessHoursRequestModel requestModel);

    Task<IReadOnlyList<BusinessHoursResponseModel>> GetAsync(Guid serviceId);
}
