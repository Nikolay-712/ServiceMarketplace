namespace ServiceMarketplace.Models.Response;

public record SubCategoryDetailsResponseModel(
    Guid Id,
    string NameBg,
    string NameEn,
    string DescriptionBg,
    string DescriptionEn,
    string CreatedOn,
    string? ModifiedOn,
    IReadOnlyList<TagResponseModel> Tags);

