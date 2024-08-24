using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models.Response.Categories;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Controllers.Users;

[Route("api/[controller]")]
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
        IReadOnlyList<CategoryResponseModel> categories = await _categoryService.GetAllAsync();
        return new ResponseContent<IReadOnlyList<CategoryResponseModel>>
        {
            Result = categories
        };
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<SubCategoryResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<SubCategoryResponseModel>>> GetAllSubCategoriesAsync([FromRoute] Guid categoryId)
    {
        IReadOnlyList<SubCategoryResponseModel> subCategories = await _categoryService.GetAllSubCategoriesAsync(categoryId);
        return new ResponseContent<IReadOnlyList<SubCategoryResponseModel>>
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
