namespace ServiceMarketplace.Models.Response;

public static class RatingResponseModels
{
    public record UserVoteResponseModel(
        Guid Id, 
        int Value, 
        string CreatedOn,
        string? ModifiedOn, 
        string? Comment, 
        OwnerCommentResponseModel? OwnerComment);

    public record OwnerCommentResponseModel(
        int Id,
        string Comment, 
        string CreatedOn, 
        string? ModifiedOn);
}
