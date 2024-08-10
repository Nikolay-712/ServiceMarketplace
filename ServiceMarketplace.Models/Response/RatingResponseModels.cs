namespace ServiceMarketplace.Models.Response;

public static class RatingResponseModels
{
    public record UserVoteResponseModel(
        Guid Id,
        int Value,
        string UserName,
        string CreatedOn,
        string? ModifiedOn,
        string? Comment,
        OwnerCommentResponseModel? OwnerComment);

    public record OwnerCommentResponseModel(
        int Id,
        string Comment,
        string CreatedOn,
        string? ModifiedOn);

    public record RatingCalculationResponseModel(
        int VotesCount, 
        double AverageRating);
}
