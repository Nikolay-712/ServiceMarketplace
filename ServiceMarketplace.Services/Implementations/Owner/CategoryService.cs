using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
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
