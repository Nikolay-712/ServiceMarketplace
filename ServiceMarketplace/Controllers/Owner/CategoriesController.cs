using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services.Interfaces.Owner;
using Microsoft.AspNetCore.Authorization;

using static ServiceMarketplace.Models.Response.CategoryResponseModels;
using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Owner;

[Authorize(Roles = OwnerRoleName)]
[Route("api/owner/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType<ResponseContent<IReadOnlyList<CategoryResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<CategoryResponseModel>>> GetAllCategoriesAsync()
    {
        IReadOnlyList<CategoryResponseModel> categories = await _categoryService.GetAllCategoriesAsync();
        return new ResponseContent<IReadOnlyList<CategoryResponseModel>>
        {
            Result = categories
        };
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<SubCategoryDetailsResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<SubCategoryDetailsResponseModel>>> GetAllSubCategoriesAsync([FromRoute] Guid categoryId)
    {
        IReadOnlyList<SubCategoryDetailsResponseModel> subCategories = await _categoryService.GetAllSubCategoriesAsync(categoryId);
        return new ResponseContent<IReadOnlyList<SubCategoryDetailsResponseModel>>
        {
            Result = subCategories
        };
    }

    [HttpGet("tags/{subCategoryId}")]
    [ProducesResponseType<IReadOnlyList<IReadOnlyList<TagResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<TagResponseModel>>> GetAllTagsAsync([FromRoute] Guid subCategoryId)
    {
        IReadOnlyList<TagResponseModel> tags = await _categoryService.GetAllTagsAsync(subCategoryId);
        return new ResponseContent<IReadOnlyList<TagResponseModel>>
        {
            Result = tags
        };
    }
}
