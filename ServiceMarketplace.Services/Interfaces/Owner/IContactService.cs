using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IContactService
{
    void CreateContactAsync(Guid serviceId, ManageContactRequestModel requestModel);

    Task UpdateContactAsync(int contactId, Guid serviceId, ManageContactRequestModel requestModel);
}
