using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Users;

using static ServiceMarketplace.Models.Response.RatingResponseModels;

namespace ServiceMarketplace.Controllers.Users;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RateServiceAsync(AddRatingRequestModel requestModel)
    {
        await _ratingService.AddOrUpdateRatingAsync(ClaimsPrincipalExtensions.GetUserId(this.User), requestModel);
        return new ResponseContent();
    }

    [HttpGet("{serviceId}")]
    [ProducesResponseType<ResponseContent<UserVoteResponseModel>>(200)]
    public async Task<ResponseContent<UserVoteResponseModel?>> GetUserVoteForServiceAsync(Guid serviceId)
    {
        UserVoteResponseModel? rating = await _ratingService.GetUserVoteForServiceAsync(ClaimsPrincipalExtensions.GetUserId(this.User), serviceId);
        return new ResponseContent<UserVoteResponseModel?>
        {
            Result = rating,
        };
    }
}
