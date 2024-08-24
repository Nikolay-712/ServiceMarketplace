using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Exceptions.ServerExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Services.Implementations.Administration;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRoleService _roleService;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, IRoleService roleService, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _roleService = roleService;
        _logger = logger;
    }

    public async Task<UserResponseModel> GetByIdAsync(Guid userId)
    {
        ApplicationUser user = await FindUserAsync(userId);
        IList<string> userRoles = await _userManager.GetRolesAsync(user);

        UserResponseModel userResponse = new(
            user.Id,
            user.FullName,
            user.Email!,
            user.CreatedOn.DateFormat(),
            userRoles);

        return userResponse;
    }

    public async Task AssignUserToRoleAsync(AssignUserToRoleRequestModel requestModel)
    {
        ApplicationUser user = await FindUserAsync(requestModel.UserId);
        ApplicationRole role = await _roleService.FindByIdAsync(requestModel.RoleId);

        bool isInRole = await _userManager.IsInRoleAsync(user, role.Name!);
        if (isInRole)
        {
            _logger.LogError("User with ID : {userId} is in role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
            throw new UserRoleExistsException(Messages.UserRoleExists);
        }

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, role.Name!);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded assign user with ID : {userId} to role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
    }

    public async Task RemoveUserFromRoleAsync(RemoveUserFromRoleRequestModel requestModel)
    {
        ApplicationUser user = await FindUserAsync(requestModel.UserId);
        ApplicationRole role = await _roleService.FindByNameAsync(requestModel.RoleName);

        bool isInRole = await _userManager.IsInRoleAsync(user, role.Name!);
        if (!isInRole)
        {
            _logger.LogError("User with ID : {UserId} is not in role with name : {RoleName}", requestModel.UserId, requestModel.RoleName);
            throw new NotFoundEntityException(Messages.UserNotInRole);
        }

        IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded remove user with ID : {UserId} from role with name : {RoleName}", requestModel.UserId, requestModel.RoleName);
    }


    private async Task<ApplicationUser> FindUserAsync(Guid userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            _logger.LogError("Not found user with ID: {userId}", userId);
            throw new NotFoundEntityException(Messages.NotFoundUser);
        }

        return user;
    }
}
