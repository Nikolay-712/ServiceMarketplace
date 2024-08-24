using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Exceptions.ServerExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Services.Implementations.Administration;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<RoleService> _logger;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, 
        ApplicationContext applicationContext,
        ILogger<RoleService> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task CreateAsync(ManageRoleRequestModel requestModel)
    {
        bool existsRole = await _roleManager.RoleExistsAsync(requestModel.Name);
        if (existsRole)
        {
            _logger.LogError("Role with name {roleName} already exists", requestModel.Name);
            throw new ExistsRoleException(Messages.RoleNameAlreadyExists);
        }

        ApplicationRole role = new()
        {
            Name = requestModel.Name,
            DescriptionEn = requestModel.DescriptionEn,
            DescriptionBg = requestModel.DescriptionBg,
        };

        IdentityResult identityResult = await _roleManager.CreateAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded create new role with name: {roleName}", requestModel.Name);
    }

    public async Task<RoleResponseModel> GetByIdAsync(Guid id)
    {
        ApplicationRole role = await FindByIdAsync(id);
        RoleResponseModel roleResponse = role.ToRoleResponseModel();
        return roleResponse;
    }

    public async Task<IReadOnlyList<RoleResponseModel>> GetAllAsync()
    {
        IReadOnlyList<RoleResponseModel> roles = await _roleManager.Roles
            .Select(x => x.ToRoleResponseModel())
            .ToListAsync();

        return roles;
    }

    public async Task UpdateAsync(Guid id, ManageRoleRequestModel requestModel)
    {
        ApplicationRole role = await FindByIdAsync(id);

        bool existsName = await _applicationContext.Roles
            .AnyAsync(x => x.Name == requestModel.Name && x.Id != role.Id);
        if (existsName)
        {
            _logger.LogError("Role with name {roleName} already exists", requestModel.Name);
            throw new ExistsRoleException(Messages.RoleNameAlreadyExists);
        }

        role.Name = requestModel.Name;
        role.DescriptionEn = requestModel.DescriptionEn;
        role.DescriptionBg = requestModel.DescriptionBg;

        IdentityResult identityResult = await _roleManager.UpdateAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded update role with ID: {roleId}", id);
    }

    public async Task RemoveAsync(Guid id)
    {
        ApplicationRole role = await FindByIdAsync(id);

        bool roleIsAssignedToUser = await _applicationContext.UserRoles.AnyAsync(x => x.RoleId == id);
        if (roleIsAssignedToUser)
        {
            _logger.LogError("Role with ID : {roleId} currently in use", id);
            throw new RemoveEntityException(Messages.RoleCurrentlyUse);
        }

        IdentityResult identityResult = await _roleManager.DeleteAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded remove role with ID: {roleId}", id);
    }

    public async Task<ApplicationRole> FindByIdAsync(Guid id)
    {
        ApplicationRole? role = await _roleManager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            _logger.LogError("Not found role with present ID : {RoleId}", id);
            throw new NotFoundEntityException(Messages.RoleNotFound);
        }
        return role;
    }

    public async Task<ApplicationRole> FindByNameAsync(string name)
    {
        ApplicationRole? role = await _roleManager.FindByNameAsync(name);
        if (role is null)
        {
            _logger.LogError("Not found role with present name : {RoleName}", name);
            throw new NotFoundEntityException(Messages.RoleNotFound);
        }
        return role;
    }
}
