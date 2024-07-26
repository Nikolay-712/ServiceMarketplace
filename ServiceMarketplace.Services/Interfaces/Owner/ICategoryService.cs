using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface ICategoryService
{
    Task ValidateSubCategoryAsync(Guid subCategoryId);

    Task ValidateSelectedTagAsync(int tagId, Guid subCategoryId);

    Task<IReadOnlyList<CategoryResponseModel>> GetAllCategoriesAsync();

    Task<IReadOnlyList<SubCategoryDetailsResponseModel>> GetAllSubCategoriesAsync(Guid categoryId);

    Task<IReadOnlyList<TagResponseModel>> GetAllTagsAsync(Guid subCategoryId);

}
