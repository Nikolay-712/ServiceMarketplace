using ServiceMarketplace.Models.Response.Categories;
using ServiceMarketplace.Models.Response.Cities;
using ServiceMarketplace.Models.Response.Contacts;
using ServiceMarketplace.Models.Response.Ratings;

namespace ServiceMarketplace.Models.Response.Services;

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
