using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Response.Categories;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Services.Implementations.Users;

public class CategoryService : ICategoryService
{
    private readonly ApplicationContext _applicationContext;

    public CategoryService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IReadOnlyList<CategoryResponseModel>> GetAllAsync()
    {
        IReadOnlyList<CategoryResponseModel> categories = await _applicationContext.Categories
            .Select(x => x.ToCategoryResponseModel())
            .ToListAsync();

        return categories;
    }

    public async Task<IReadOnlyList<SubCategoryResponseModel>> GetAllSubCategoriesAsync(Guid categoryId)
    {
        IReadOnlyList<SubCategoryResponseModel> subCategories = await _applicationContext.SubCategories
            .Where(x => x.CategoryId == categoryId).Select(x => x.ToSubCategoryResponseModel())
            .ToListAsync();

        return subCategories;
    }

    public async Task<IReadOnlyList<TagResponseModel>> GetAllTagsAsync(Guid subCategory)
    {
        IReadOnlyList<TagResponseModel> tags = await _applicationContext.Tags
            .Where(x => x.SubCategoryId == subCategory).Select(x => x.ToTagResponseModel())
            .ToListAsync();

        return tags;
    }
}
