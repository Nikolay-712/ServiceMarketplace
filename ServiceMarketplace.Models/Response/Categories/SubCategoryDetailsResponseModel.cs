namespace ServiceMarketplace.Models.Response.Categories;

public record SubCategoryDetailsResponseModel(
        Guid Id,
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn,
        string CreatedOn,
        string? ModifiedOn,
        IReadOnlyList<TagResponseModel> Tags);
