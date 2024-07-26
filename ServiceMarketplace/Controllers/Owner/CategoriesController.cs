using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Controllers.Owner;

//[Authorize(Roles = "Owner")]
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
    public async Task<ResponseContent<IReadOnlyList<TagResponseModel>>> GetAllSTagsAsync([FromRoute] Guid subCategoryId)
    {
        IReadOnlyList<TagResponseModel> tags = await _categoryService.GetAllTagsAsync(subCategoryId);
        return new ResponseContent<IReadOnlyList<TagResponseModel>>
        {
            Result = tags
        };
    }
}
