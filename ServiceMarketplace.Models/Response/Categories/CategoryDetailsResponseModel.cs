namespace ServiceMarketplace.Models.Response.Categories;

public record CategoryDetailsResponseModel(
       string NameBg,
       string NameEn,
       string DescriptionBg,
       string DescriptionEn,
       string CreatedOn,
       string? ModifiedOn,
       PaginationResponseModel<SubCategoryResponseModel> SubCategories);
