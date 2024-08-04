namespace ServiceMarketplace.Models.Response;

public static class CategoryResponseModels
{
    public record CategoryResponseModel(
        Guid Id,
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn);

    public record CategoryDetailsResponseModel(
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn,
        string CreatedOn,
        string? ModifiedOn,
        PaginationResponseModel<SubCategoryResponseModel> SubCategories);

    public record SubCategoryResponseModel(
        Guid Id,
        string NameBg,
        string NameEn);

    public record SubCategoryDetailsResponseModel(
        Guid Id,
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn,
        string CreatedOn,
        string? ModifiedOn,
        IReadOnlyList<TagResponseModel> Tags);


    public record TagResponseModel(
        int Id,
        string NameBg,
        string NameEn);
}
