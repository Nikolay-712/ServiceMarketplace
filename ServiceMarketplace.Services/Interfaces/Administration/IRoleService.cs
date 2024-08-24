using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;

namespace ServiceMarketplace.Services.Interfaces.Administration;

public interface IRoleService
{
    Task CreateAsync(ManageRoleRequestModel requestModel);

    Task<RoleResponseModel> GetByIdAsync(Guid id);

    Task<IReadOnlyList<RoleResponseModel>> GetAllAsync();

    Task UpdateAsync(Guid id, ManageRoleRequestModel requestModel);

    Task RemoveAsync(Guid id);

    Task<ApplicationRole> FindByIdAsync(Guid id);

    Task<ApplicationRole> FindByNameAsync(string name);
}
