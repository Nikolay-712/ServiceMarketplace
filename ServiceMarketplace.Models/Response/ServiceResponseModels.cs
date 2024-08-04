using static ServiceMarketplace.Models.Response.CategoryResponseModels;

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
        IReadOnlyList<ContactResponseModel> Contacts);

    public record OfferedAtResponseModel(
        int Id,
        string NameBg,
        string NameEn);

    public record ContactResponseModel(
        int Id,
        string Name,
        string PhoneNumber,
        string LocationUrl);
}
