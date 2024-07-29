using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using System.Threading.Tasks;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IContactService
{
    void CreateContactAsync(Guid serviceId, ManageContactRequestModel requestModel);

    Task AddAsync(Guid serviceId, ManageContactRequestModel requestModel);

    Task<IReadOnlyList<ContactResponseModel>> GetAllAsync(Guid serviceId);

    Task UpdateAsync(int contactId, Guid serviceId, ManageContactRequestModel requestModel);

    Task RemoveAsync(int contactId, Guid serviceId);
}
