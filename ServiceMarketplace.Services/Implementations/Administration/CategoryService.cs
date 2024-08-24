using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Models.Response.Categories;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Services.Implementations.Administration;

public class CategoryService : ICategoryService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationContext applicationContext, ILogger<CategoryService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    //Categories
    public async Task CreateCategoryAsync(ManageCategoryRequestModel requestModel)
    {
        await ValidateCategoryNameAsync(requestModel.NameBg, requestModel.NameEn);
        await ValidateSubCategoryNameAsync(requestModel.NameBg, requestModel.NameEn);

        Category category = new()
        {
            NameBg = requestModel.NameBg,
            NameEn = requestModel.NameEn,
            DescriptionBg = requestModel.DescriptionBg,
            DescriptionEn = requestModel.DescriptionEn,
        };

        _applicationContext.Categories.Add(category);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added a category with ID: {CategoryId}", category.Id);
    }

    public async Task<PaginationResponseModel<CategoryResponseModel>> GetAllCategoriesAsync(CategoryFilter categoryFilter)
    {
        IQueryable<Category> categoriesQuery = _applicationContext.Categories;
        categoriesQuery = ApplyCategoryFilter(categoriesQuery, categoryFilter);

        int totalCount = await categoriesQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / categoryFilter.ItemsPerPage);

        categoriesQuery = categoriesQuery
            .Skip(categoryFilter.SkipCount)
            .Take(categoryFilter.ItemsPerPage);

        IReadOnlyList<CategoryResponseModel> categories = await categoriesQuery
              .Select(x => x.ToCategoryResponseModel())
              .ToListAsync();

        return new PaginationResponseModel<CategoryResponseModel>
        {
            Items = categories,
            TotalItems = totalCount,
            PageNumber = categoryFilter.PageNumber,
            ItemsPerPage = categoryFilter.ItemsPerPage,
            PagesCount = pagesCount,
        };
    }

    public async Task<CategoryDetailsResponseModel> GetCategoryDetailsAsync(Guid categoryId, CategoryFilter categoryFilter)
    {
        Category? category = await _applicationContext.Categories
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (category is null)
        {
            _logger.LogError("No category exists with this ID {CategoryId}", categoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        IQueryable<SubCategory> subCategoriesQuery = _applicationContext.SubCategories.Where(x => x.CategoryId == categoryId);
        PaginationResponseModel<SubCategoryResponseModel> subCategoriesPagination = await GetSubCategoriesWithPaginationAsync(subCategoriesQuery, categoryFilter);

        CategoryDetailsResponseModel categoryDetails = category.ToCategoryDetailsResponseModel(subCategoriesPagination);

        return categoryDetails;
    }

    public async Task UpdateCategoryAsync(Guid categoryId, ManageCategoryRequestModel requestModel)
    {
        Category? category = await _applicationContext.Categories
          .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (category is null)
        {
            _logger.LogError("No category exists with this ID {CategoryId}", categoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        bool existsCategoryName = await _applicationContext.Categories.AnyAsync(x
            => (x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn) && x.Id != categoryId);

        if (existsCategoryName)
        {
            _logger.LogError("A category with this name has already been added {CategoryNameBg}/{CategoryNameEn}",
                requestModel.NameBg,
                requestModel.NameEn);
            throw new ExistsCategoryNameException(Messages.ExistsCategoryName);
        }

        await ValidateSubCategoryNameAsync(requestModel.NameBg, requestModel.NameEn);

        category.NameBg = requestModel.NameBg;
        category.NameEn = requestModel.NameEn;
        category.DescriptionBg = requestModel.DescriptionBg;
        category.DescriptionEn = requestModel.DescriptionEn;
        category.ModifiedOn = DateTime.UtcNow;

        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully updated a category with ID: {CategoryId}", category.Id);
    }


    //Sub categories
    public async Task CreateSubCategoryAsync(ManageSubCategoryRequestModel requestModel)
    {
        bool existsCategory = await _applicationContext.Categories.AnyAsync(x => x.Id == requestModel.CategoryId);
        if (!existsCategory)
        {
            _logger.LogError("No category exists with this ID {CategoryId}", requestModel.CategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        bool existsSubCategoryName = await _applicationContext.SubCategories
            .Where(x => x.CategoryId == requestModel.CategoryId)
            .AnyAsync(x => x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn);

        if (existsSubCategoryName)
        {
            _logger.LogError("A sub-category with this name has already been added {SubCategoryNameBg}/{SubCategoryNameEn}",
                requestModel.NameBg,
                requestModel.NameEn);
            throw new ExistsCategoryNameException(Messages.ExistsSubCategoryName);
        }

        await ValidateCategoryNameAsync(requestModel.NameBg, requestModel.NameEn);

        SubCategory subCategory = new()
        {
            NameBg = requestModel.NameBg,
            NameEn = requestModel.NameEn,
            DescriptionBg = requestModel.DescriptionBg,
            DescriptionEn = requestModel.DescriptionEn,
            CategoryId = requestModel.CategoryId,
        };

        _applicationContext.SubCategories.Add(subCategory);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added a sub-category with ID: {SubCategoryId}", subCategory.Id);
    }

    public async Task<PaginationResponseModel<SubCategoryResponseModel>> GetAllSubCategoriesAsync(CategoryFilter categoryFilter)
    {
        IQueryable<SubCategory> subCategoriesQuery = _applicationContext.SubCategories;
        PaginationResponseModel<SubCategoryResponseModel> subCategoriesPagination = await GetSubCategoriesWithPaginationAsync(subCategoriesQuery, categoryFilter);

        return subCategoriesPagination;
    }

    public async Task<SubCategoryDetailsResponseModel> GetSubCategoryDetailsAsync(Guid subCategoryId)
    {
        SubCategory? subCategory = await _applicationContext.SubCategories
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == subCategoryId);

        if (subCategory is null)
        {
            _logger.LogError("No sub-category exists with this ID {SubCategoryId}", subCategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        IReadOnlyList<TagResponseModel> tags = subCategory.Tags
            .Select(x => x.ToTagResponseModel())
            .ToList();
        SubCategoryDetailsResponseModel subCategoryDetails = subCategory.ToSubCategoryDetailsResponseModel(tags);

        return subCategoryDetails;
    }

    public async Task UpdateSubCategoryAsync(Guid subCategoryId, ManageSubCategoryRequestModel requestModel)
    {
        SubCategory? subCategory = await _applicationContext.SubCategories.FirstOrDefaultAsync(x => x.Id == subCategoryId);
        if (subCategory is null)
        {
            _logger.LogError("No sub-category exists with this ID {CategoryId}", subCategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        bool existsCategory = await _applicationContext.Categories.AnyAsync(x => x.Id == requestModel.CategoryId);
        if (!existsCategory)
        {
            _logger.LogError("No category exists with this ID {CategoryId}", requestModel.CategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }

        bool existsSubCategoryName = await _applicationContext.SubCategories
            .Where(x => x.CategoryId == requestModel.CategoryId)
            .AnyAsync(x => (x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn) && x.Id != subCategoryId);

        if (existsSubCategoryName)
        {
            _logger.LogError("A sub-category with this name has already been added {SubCategoryNameBg}/{SubCategoryNameEn}",
                requestModel.NameBg,
                requestModel.NameEn);
            throw new ExistsCategoryNameException(Messages.ExistsSubCategoryName);
        }

        await ValidateCategoryNameAsync(requestModel.NameBg, requestModel.NameEn);

        subCategory.NameBg = requestModel.NameBg;
        subCategory.NameEn = requestModel.NameEn;
        subCategory.DescriptionBg = requestModel.DescriptionBg;
        subCategory.DescriptionEn = requestModel.DescriptionEn;
        subCategory.CategoryId = requestModel.CategoryId;
        subCategory.ModifiedOn = DateTime.UtcNow;

        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added a sub-category with ID: {SubCategoryId}", subCategory.Id);
    }

    //Tags
    public async Task CreateTagAsync(CreateTagRequestModel requestModel)
    {
        bool existsSubCategory = await _applicationContext.SubCategories.AnyAsync(x => x.Id == requestModel.SubCategoryId);
        if (!existsSubCategory)
        {
            _logger.LogError("No sub-category exists with this ID {CategoryId}", requestModel.SubCategoryId);
            throw new NotFoundEntityException(Messages.NotFoundCategory);
        }
        bool existsTag = await _applicationContext.Tags
            .AnyAsync(x =>
                (x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn)
                && x.SubCategoryId == requestModel.SubCategoryId);

        if (existsTag)
        {
            _logger.LogError("A tag with this name has already been added {TagNameBg}/{TagNameEn}", requestModel.NameBg, requestModel.NameEn);
            throw new ExistsTagException(Messages.ExistsTag);
        }

        Tag tag = new()
        {
            NameBg = requestModel.NameBg,
            NameEn = requestModel.NameEn,
            SubCategoryId = requestModel.SubCategoryId,
        };

        _applicationContext.Tags.Add(tag);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added a tag with ID: {TagId}", tag.Id);
    }

    public async Task RemoveTagAsync(int id, Guid subCategoryId)
    {
        Tag? tag = await _applicationContext.Tags.FirstOrDefaultAsync(x => x.Id == id && x.SubCategoryId == subCategoryId);
        if (tag is null)
        {
            _logger.LogError("No tag exists with this ID {TagId}", id);
            throw new NotFoundEntityException(Messages.NotFoundTag);
        }

        _applicationContext.Tags.Remove(tag);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully removed a tag with ID: {TagId}", tag.Id);
    }

    private async Task ValidateCategoryNameAsync(string nameBg, string nameEn)
    {
        bool existsCategoryName = await _applicationContext.Categories.AnyAsync(x => x.NameBg == nameBg || x.NameEn == nameEn);
        if (existsCategoryName)
        {
            _logger.LogError("A category with this name has already been added {CategoryNameBg}/{CategoryNameEn}", nameBg, nameEn);
            throw new ExistsCategoryNameException(Messages.ExistsCategoryName);
        }
    }

    private async Task ValidateSubCategoryNameAsync(string nameBg, string nameEn)
    {
        bool existsSubCategoryName = await _applicationContext.SubCategories.AnyAsync(x => x.NameBg == nameBg || x.NameEn == nameEn);
        if (existsSubCategoryName)
        {
            _logger.LogError("A sub-category with this name has already been added {SubCategoryNameBg}/{SubCategoryNameEn}", nameBg, nameEn);
            throw new ExistsCategoryNameException(Messages.ExistsSubCategoryName);
        }
    }

    private IQueryable<Category> ApplyCategoryFilter(IQueryable<Category> categoriesQuery, CategoryFilter categoryFilter)
    {
        if (categoryFilter.SearchTerm is not null)
        {
            categoriesQuery = categoriesQuery
                .Where(x => x.NameBg.Contains(categoryFilter.SearchTerm) || x.NameEn.Contains(categoryFilter.SearchTerm));
        }

        //ToDo implement logic to order by en category name
        categoriesQuery = categoryFilter.OrderParameters switch
        {
            OrderParameters.Name => categoryFilter.Ascending
                ? categoriesQuery.OrderBy(x => x.NameBg)
                : categoriesQuery.OrderByDescending(x => x.NameBg),
            OrderParameters.Data => categoryFilter.Ascending
                ? categoriesQuery.OrderBy(x => x.CreatedOn)
                : categoriesQuery.OrderByDescending(x => x.CreatedOn),
            _ => categoryFilter.Ascending is true
                ? categoriesQuery.OrderBy(x => x.NameBg)
                : categoriesQuery.OrderByDescending(x => x.NameBg)
        };

        return categoriesQuery;
    }

    private IQueryable<SubCategory> ApplySubCategoryFilters(IQueryable<SubCategory> subCategories, CategoryFilter categoryFilter)
    {
        if (categoryFilter.SearchTerm is not null)
        {
            subCategories = subCategories
                .Where(x => x.NameBg.Contains(categoryFilter.SearchTerm) || x.NameEn.Contains(categoryFilter.SearchTerm));
        }

        //ToDo implement logic to order by en category name
        subCategories = categoryFilter.OrderParameters switch
        {
            OrderParameters.Name => categoryFilter.Ascending
                ? subCategories.OrderBy(x => x.NameBg)
                : subCategories.OrderByDescending(x => x.NameBg),
            OrderParameters.Data => categoryFilter.Ascending
                ? subCategories.OrderBy(x => x.CreatedOn)
                : subCategories.OrderByDescending(x => x.CreatedOn),
            _ => categoryFilter.Ascending is true
                ? subCategories.OrderBy(x => x.NameBg)
                : subCategories.OrderByDescending(x => x.NameBg)
        };

        return subCategories;
    }

    private async Task<PaginationResponseModel<SubCategoryResponseModel>> GetSubCategoriesWithPaginationAsync(IQueryable<SubCategory> subCategoriesQuery, CategoryFilter categoryFilter)
    {
        subCategoriesQuery = ApplySubCategoryFilters(subCategoriesQuery, categoryFilter);

        int totalCount = await subCategoriesQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / categoryFilter.ItemsPerPage);

        subCategoriesQuery = subCategoriesQuery
            .Skip(categoryFilter.SkipCount)
            .Take(categoryFilter.ItemsPerPage);

        IReadOnlyList<SubCategoryResponseModel> subCategories = await subCategoriesQuery
            .Select(x => x.ToSubCategoryResponseModel())
            .ToListAsync();

        PaginationResponseModel<SubCategoryResponseModel> subCategoriesPagination = new()
        {
            Items = subCategories,
            TotalItems = totalCount,
            PageNumber = categoryFilter.PageNumber,
            ItemsPerPage = categoryFilter.ItemsPerPage,
            PagesCount = pagesCount
        };

        return subCategoriesPagination;
    }
}
