using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IServiceService
{
    Task CreateAsync(Guid ownerId, CreateServiceRequestModel requestModel);

    Task UpdateAsync(Guid serviceId, Guid ownerId, UpdateServiceRequestModel requestModel);

    Task<IReadOnlyList<ServiceResponseModel>> GetAllAsync(Guid ownerId);

    Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid ownerId, Guid serviceId);

    Task ChangeCategoryAsync(Guid serviceId, Guid ownerId, ChangeCategoryRequestModel requestModel);

    Task AddTagAsync(Guid serviceId, Guid ownerId, int tagId);

    Task RemoveTagAsync(Guid serviceId, Guid ownerId, int tagId);

    Task RemoveCityAsync(Guid serviceId, Guid ownerId, Guid cityId);

    Task<IReadOnlyList<OfferedAtResponseModel>> GetOfferedAtOptionsAsync();
}
