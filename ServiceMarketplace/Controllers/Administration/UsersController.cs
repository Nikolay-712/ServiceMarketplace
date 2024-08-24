using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Controllers.Administration;

[Route("api/administration/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType<ResponseContent<UserResponseModel>>(200)]
    public async Task<ResponseContent<UserResponseModel>> GetByIdAsync([FromRoute] Guid userId)
    {
        UserResponseModel user = await _userService.GetByIdAsync(userId);
        return new ResponseContent<UserResponseModel>
        {
            Result = user,
        };
    }

    [HttpPost("assign-role")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> AssignToRoleAsync(AssignUserToRoleRequestModel requestModel)
    {
        await _userService.AssignUserToRoleAsync(requestModel);
        return new ResponseContent();
    }

    [HttpPost("remove-role")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveFromRoleAsync(RemoveUserFromRoleRequestModel requestModel)
    {
        await _userService.RemoveUserFromRoleAsync(requestModel);
        return new ResponseContent();
    }
}
