using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Models.Response.Categories;


namespace ServiceMarketplace.Models.Extensions;

public static class CategoryMappingExtensions
{
    public static CategoryResponseModel ToCategoryResponseModel(this Category category)
        => new(category.Id,
               category.NameBg,
               category.NameEn,
               category.DescriptionBg,
               category.DescriptionEn);

    public static CategoryDetailsResponseModel ToCategoryDetailsResponseModel(this Category category, PaginationResponseModel<SubCategoryResponseModel> subCategoriesPagination)
        => new(category.NameBg,
               category.NameEn,
               category.DescriptionBg,
               category.DescriptionEn,
               category.CreatedOn.DateFormat(),
               category.ModifiedOn is null
                   ? string.Empty
                   : category.ModifiedOn.Value.DateFormat(),
               subCategoriesPagination);

    public static SubCategoryResponseModel ToSubCategoryResponseModel(this SubCategory subCategory)
        => new(subCategory.Id,
               subCategory.NameBg,
               subCategory.NameEn);

    public static SubCategoryDetailsResponseModel ToSubCategoryDetailsResponseModel(this SubCategory subCategory, IReadOnlyList<TagResponseModel> tags)
        => new(subCategory.Id,
               subCategory.NameBg,
               subCategory.NameEn,
               subCategory.DescriptionBg,
               subCategory.DescriptionEn,
               subCategory.CreatedOn.DateFormat(),
               subCategory.ModifiedOn is null
                   ? string.Empty
                   : subCategory.ModifiedOn.Value.DateFormat(),
               tags);

    public static TagResponseModel ToTagResponseModel(this Tag tag)
        => new(tag.Id,
               tag.NameBg,
               tag.NameEn);
}
