using ServiceMarketplace.Models.Response.Categories;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponseModel>> GetAllAsync();

    Task<IReadOnlyList<SubCategoryResponseModel>> GetAllSubCategoriesAsync(Guid categoryId);

    Task<IReadOnlyList<TagResponseModel>> GetAllTagsAsync(Guid subCategory);
}
