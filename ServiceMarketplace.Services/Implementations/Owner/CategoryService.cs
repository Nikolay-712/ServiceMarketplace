using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class CategoryService : ICategoryService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationContext applicationContext, ILogger<CategoryService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<CategoryResponseModel>> GetAllCategoriesAsync()
    {
        IReadOnlyList<CategoryResponseModel> categories = await _applicationContext.Categories
            .OrderBy(x => x.NameBg)
            .ThenBy(x => x.NameEn)
            .Select(x => x.ToCategoryResponseModel())
            .ToListAsync();

        return categories;
    }

    public async Task<IReadOnlyList<SubCategoryDetailsResponseModel>> GetAllSubCategoriesAsync(Guid categoryId)
    {
        IQueryable<SubCategory> subCategoriesQuery = _applicationContext.SubCategories.Where(x => x.CategoryId == categoryId);

        IReadOnlyList<SubCategoryDetailsResponseModel> subCategories = await subCategoriesQuery
            .OrderBy(x => x.NameBg)
            .Select(x => x.ToSubCategoryDetailsResponseModel(new List<TagResponseModel>()))
            .ToListAsync();

        return subCategories;
    }

    public async Task<IReadOnlyList<TagResponseModel>> GetAllTagsAsync(Guid subCategoryId)
    {
        IReadOnlyList<TagResponseModel> tags = await _applicationContext.Tags
             .Where(x => x.SubCategoryId == subCategoryId)
             .OrderBy(x => x.NameBg)
             .Select(x => x.ToTagResponseModel())
             .ToListAsync();

        return tags;
    }

    public async Task ValidateSubCategoryAsync(Guid subCategoryId)
    {
        bool existsSubCategory = await _applicationContext.SubCategories.AnyAsync(x => x.Id == subCategoryId);
        if (!existsSubCategory)
        {
            _logger.LogError("No sub-category exists with this ID {CategoryId}", subCategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }
    }

    public async Task ValidateSelectedTagAsync(int tagId, Guid subCategoryId)
    {
        bool isValidTag = await _applicationContext.Tags.AnyAsync(x => x.Id == tagId && x.SubCategoryId == subCategoryId);
        if (!isValidTag)
        {
            _logger.LogError("No tag exists with this ID {TagId} and sub-category ID {SubCategoryId}", tagId, subCategoryId);
            throw new NotFoundEntityException(Messages.NotFoundTag);
        }
    }
}
