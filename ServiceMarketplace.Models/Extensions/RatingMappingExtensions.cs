using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Models.Response.Ratings;


namespace ServiceMarketplace.Models.Extensions;

public static class RatingMappingExtensions
{
    public static OwnerCommentResponseModel ToOwnerCommentResponse(this OwnerComment ownerComment)
        => new (ownerComment.Id,
                ownerComment.Comment,
                ownerComment.CreatedOn.DateFormat(),
                ownerComment.ModifiedOn?.DateFormat());

    public static UserVoteResponseModel ToUserVoteResponseModel(this Rating rating) 
        => new (rating.Id,
                rating.Value,
                rating.User.UserName!,
                rating.CreatedOn.DateFormat(),
                rating.ModifiedOn?.DateFormat(),
                rating.Comment,
                rating.OwnerComment?.ToOwnerCommentResponse());
}
