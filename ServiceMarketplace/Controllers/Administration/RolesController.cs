using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;
using ServiceMarketplace.Services.Interfaces.Administration;

using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Administration;

//[Authorize(Roles = AdminRoleName)]
[Route("api/administration/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateAsync(ManageRoleRequestModel requestModel)
    {
        await _roleService.CreateAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("{roleId}")]
    [ProducesResponseType<ResponseContent<RoleResponseModel>>(200)]
    public async Task<ResponseContent<RoleResponseModel>> GetByIdAsync([FromRoute] Guid roleId)
    {
        RoleResponseModel role = await _roleService.GetByIdAsync(roleId);
        return new ResponseContent<RoleResponseModel>
        {
            Result = role
        };
    }

    [HttpGet("all")]
    public async Task<ResponseContent<IReadOnlyList<RoleResponseModel>>> GetAllAsync()
    {
        IReadOnlyList<RoleResponseModel> roles = await _roleService.GetAllAsync();
        return new ResponseContent<IReadOnlyList<RoleResponseModel>>
        {
            Result = roles
        };
    }

    [HttpPut("update/{roleId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateAsync([FromRoute] Guid roleId, ManageRoleRequestModel requestModel)
    {
        await _roleService.UpdateAsync(roleId, requestModel);
        return new ResponseContent();
    }

    [HttpDelete("remove/{roleId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveAsync([FromRoute] Guid roleId)
    {
        await _roleService.RemoveAsync(roleId);
        return new ResponseContent();
    }

}
