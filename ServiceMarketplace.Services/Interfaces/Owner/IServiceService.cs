using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IServiceService
{
    Task CreateAsync(Guid ownerId, CreateServiceRequestModel requestModel);
}
