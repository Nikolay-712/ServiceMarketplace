using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Infrastructure.Extensions;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Owner;

[Authorize(Roles = OwnerRoleName)]
[Route("api/owner/[controller]")]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost("owner-comment")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> SendOwnerCommentAsync(SendOwnerCommentRequestModel requestModel)
    {
        
        await _ratingService.SendOwnerCommentAsync(ClaimsPrincipalExtensions.GetUserId(this.User), requestModel);
        return new ResponseContent();
    }

    [HttpDelete("remove-comment/{commentId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveOwnerCommentAsync(int commentId)
    {
        await _ratingService.RemoveOwnerCommentAsync(ClaimsPrincipalExtensions.GetUserId(this.User), commentId);
        return new ResponseContent();
    }
}
