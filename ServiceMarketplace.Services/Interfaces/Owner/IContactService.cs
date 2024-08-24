using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Contacts;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IContactService
{
    void CreateContactAsync(Guid serviceId, ManageContactRequestModel requestModel);

    Task AddAsync(Guid serviceId, ManageContactRequestModel requestModel, Guid ownerId);

    Task<IReadOnlyList<ContactResponseModel>> GetAllAsync(Guid serviceId);

    Task UpdateAsync(int contactId, Guid serviceId, Guid ownerId, ManageContactRequestModel requestModel);

    Task RemoveAsync(int contactId, Guid serviceId, Guid ownerId);
}
