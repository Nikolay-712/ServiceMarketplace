namespace ServiceMarketplace.Models.Response.Ratings;

public record OwnerCommentResponseModel(
       int Id,
       string Comment,
       string CreatedOn,
       string? ModifiedOn);