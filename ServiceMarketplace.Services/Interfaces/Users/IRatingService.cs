using ServiceMarketplace.Models.Request;
using static ServiceMarketplace.Models.Response.RatingResponseModels;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IRatingService
{
    Task AddOrUpdateRatingAsync(Guid userId, AddRatingRequestModel requestModel);

    Task<UserVoteResponseModel?>  GetUserVoteForServiceAsync(Guid userId, Guid serviceId);
}
