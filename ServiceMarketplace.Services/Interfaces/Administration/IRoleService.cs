using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Administration;

public interface IRoleService
{
    Task CreateAsync(ManageRoleRequestModel requestModel);

    Task<RoleResponseModel> GetByIdAsync(Guid id);

    Task<IReadOnlyList<RoleResponseModel>> GetAllAsync();

    Task UpdateAsync(Guid id, ManageRoleRequestModel requestModel);

    Task RemoveAsync(Guid id);
}
