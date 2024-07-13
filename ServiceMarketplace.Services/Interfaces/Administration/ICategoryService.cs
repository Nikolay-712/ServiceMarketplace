using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Administration;

public interface ICategoryService
{
    Task CreateCategoryAsync(ManageCategoryRequestModel requestModel);

    Task<PaginationResponseModel<CategoryResponseModel>> GetAllCategoriesAsync(CategoryFilter categoryFilter);

    Task<CategoryDetailsResponseModel> GetCategoryDetailsAsync(Guid categoryId, CategoryFilter categoryFilter);

    Task UpdateCategoryAsync(Guid categoryId, ManageCategoryRequestModel requestModel);

    Task CreateSubCategoryAsync(ManageSubCategoryRequestModel requestModel);

    Task<PaginationResponseModel<SubCategoryResponseModel>> GetAllSubCategoriesAsync(CategoryFilter categoryFilter);

    Task<SubCategoryDetailsResponseModel> GetSubCategoryDetailsAsync(Guid subCategoryId);

    Task UpdateSubCategoryAsync(Guid subCategoryId, ManageSubCategoryRequestModel requestModel);

    Task CreateTagAsync(CreateTagRequestModel requestModel);

    Task RemoveTagAsync(int id, Guid subCategoryId);
}
