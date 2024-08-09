using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Controllers.Users;

//[Authorize]
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
        Guid userId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _ratingService.AddOrUpdateRatingAsync(userId, requestModel);
        return new ResponseContent();
    }
}
