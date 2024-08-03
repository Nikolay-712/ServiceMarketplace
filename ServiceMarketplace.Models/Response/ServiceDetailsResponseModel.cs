namespace ServiceMarketplace.Models.Response;

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

