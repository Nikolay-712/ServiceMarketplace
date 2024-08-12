using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Controllers.Owner;

//[Authorize(Roles = "Owner")]
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
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _ratingService.SendOwnerCommentAsync(ownerId, requestModel);
        return new ResponseContent();
    }

    [HttpDelete("remove-comment/{commentId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveOwnerCommentAsync(int commentId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _ratingService.RemoveOwnerCommentAsync(ownerId, commentId);
        return new ResponseContent();
    }
}
