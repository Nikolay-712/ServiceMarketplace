using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Administration;

public interface IUserService
{
    Task<UserResponseModel> GetByIdAsync(Guid userId);

    Task AssignUserToRoleAsync(AssignUserToRoleRequestModel requestModel);

    Task RemoveUserFromRoleAsync(RemoveUserFromRoleRequestModel requestModel);
}
