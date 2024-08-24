
namespace ServiceMarketplace.Models.Response.Ratings;

public record RatingResponseModel(
        PaginationResponseModel<UserVoteResponseModel> UserVotes,
        int VotesCount,
        double AverageRating);