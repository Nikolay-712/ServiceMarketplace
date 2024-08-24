using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models.Response.Ratings;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IRatingService
{
    Task AddOrUpdateRatingAsync(Guid userId, AddRatingRequestModel requestModel);

    Task<UserVoteResponseModel?>  GetUserVoteForServiceAsync(Guid userId, Guid serviceId);

    Task<PaginationResponseModel<UserVoteResponseModel>> GetServiceRatingAsync(Guid serviceId, RatingFilter ratingFilter);

    Task<RatingCalculationResponseModel> CalculateServiceRatingAsync(Guid serviceId);
}
