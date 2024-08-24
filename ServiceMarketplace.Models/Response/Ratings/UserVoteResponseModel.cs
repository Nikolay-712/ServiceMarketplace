namespace ServiceMarketplace.Models.Response.Ratings;

public record UserVoteResponseModel(
        Guid Id,
        int Value,
        string UserName,
        string CreatedOn,
        string? ModifiedOn,
        string? Comment,
        OwnerCommentResponseModel? OwnerComment);
