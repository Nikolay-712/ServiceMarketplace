using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models;
using static ServiceMarketplace.Models.Response.RatingResponseModels;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;
using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface IRatingService
{
    Task<PaginationResponseModel<UserVoteResponseModel>> GetServiceRatingAsync(Guid serviceId, RatingFilter ratingFilter);

    Task<RatingCalculationResponseModel> CalculateServiceRatingAsync(Guid serviceId);

    RatingResponseModel CreateRatingResponse(PaginationResponseModel<UserVoteResponseModel> userVotes, RatingCalculationResponseModel calculation);

    Task SendOwnerCommentAsync(Guid ownerId, SendOwnerCommentRequestModel requestModel);

    Task RemoveOwnerCommentAsync(Guid ownerId, int commentId);
}
