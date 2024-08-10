using static ServiceMarketplace.Models.Response.CategoryResponseModels;
using static ServiceMarketplace.Models.Response.RatingResponseModels;

namespace ServiceMarketplace.Models.Response;

public static class ServiceResponseModels
{
    public record ServiceResponseModel(
        Guid Id,
        string CreatedOn,
        string NameBg,
        string NameEn);

    public record ServiceDetailsResponseModel(
        Guid Id,
        string CreatedOn,
        string? ModifiedOn,
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn,
        SubCategoryResponseModel SubCategory,
        OfferedAtResponseModel OfferedAt,
        IReadOnlyList<CityResponseModel> Cities,
        IReadOnlyList<TagResponseModel> Tags,
        IReadOnlyList<ContactResponseModel> Contacts,
        RatingResponseModel Ratings);

    public record OfferedAtResponseModel(
        int Id,
        string NameBg,
        string NameEn);

    public record ContactResponseModel(
        int Id,
        string Name,
        string PhoneNumber,
        string LocationUrl);

    public record RatingResponseModel(
        PaginationResponseModel<UserVoteResponseModel> UserVotes, 
        int VotesCount, 
        double AverageRating);
}
