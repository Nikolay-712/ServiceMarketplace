using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Administration;

using static ServiceMarketplace.Models.Response.CategoryResponseModels;

namespace ServiceMarketplace.Controllers.Administration;

//[Authorize(Roles = "Administrator")]
[Route("api/administration/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("create")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateCategoryAsync(ManageCategoryRequestModel requestModel)
    {
        await _categoryService.CreateCategoryAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("all")]
    [ProducesResponseType<ResponseContent<PaginationResponseModel<CategoryResponseModel>>>(200)]
    public async Task<ResponseContent<PaginationResponseModel<CategoryResponseModel>>> GetAllCategoriesAsync([FromQuery] CategoryFilter categoryFilter)
    {
        PaginationResponseModel<CategoryResponseModel> categories = await _categoryService.GetAllCategoriesAsync(categoryFilter);
        return new ResponseContent<PaginationResponseModel<CategoryResponseModel>>
        {
            Result = categories
        };
    }

    [HttpGet("details/{categoryId}")]
    [ProducesResponseType<ResponseContent<CategoryDetailsResponseModel>>(200)]
    public async Task<ResponseContent<CategoryDetailsResponseModel>> GetCategoryDetailsAsync(Guid categoryId, [FromQuery] CategoryFilter categoryFilter)
    {
        CategoryDetailsResponseModel categoryDetails = await _categoryService.GetCategoryDetailsAsync(categoryId, categoryFilter);
        return new ResponseContent<CategoryDetailsResponseModel>
        {
            Result = categoryDetails
        };
    }

    [HttpPut("update/{categoryId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateCategoryAsycn(Guid categoryId, ManageCategoryRequestModel requestModel)
    {
        await _categoryService.UpdateCategoryAsync(categoryId, requestModel);
        return new ResponseContent();
    }

    [HttpPost("sub-create")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateSubCategoryAsync(ManageSubCategoryRequestModel requestModel)
    {
        await _categoryService.CreateSubCategoryAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("sub-all")]
    [ProducesResponseType<ResponseContent<PaginationResponseModel<SubCategoryResponseModel>>>(200)]
    public async Task<ResponseContent<PaginationResponseModel<SubCategoryResponseModel>>> GetAllSubCategoriesAsync([FromQuery] CategoryFilter categoryFilter)
    {
        PaginationResponseModel<SubCategoryResponseModel> subCategories = await _categoryService.GetAllSubCategoriesAsync(categoryFilter);
        return new ResponseContent<PaginationResponseModel<SubCategoryResponseModel>>
        {
            Result = subCategories
        };
    }

    [HttpGet("sub-details/{subCategoryId}")]
    [ProducesResponseType<ResponseContent<SubCategoryDetailsResponseModel>>(200)]
    public async Task<ResponseContent<SubCategoryDetailsResponseModel>> GetSubCategoryDetailsAsync(Guid subCategoryId)
    {
        SubCategoryDetailsResponseModel subCategoryDetails = await _categoryService.GetSubCategoryDetailsAsync(subCategoryId);
        return new ResponseContent<SubCategoryDetailsResponseModel>
        {
            Result = subCategoryDetails
        };
    }

    [HttpPut("sub-update/{subCategoryId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateSubCategoryAsync(Guid subCategoryId, ManageSubCategoryRequestModel requestModel)
    {
        await _categoryService.UpdateSubCategoryAsync(subCategoryId, requestModel);
        return new ResponseContent();
    }

    [HttpPost("create-tag")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateTagASync(CreateTagRequestModel requestModel)
    {
        await _categoryService.CreateTagAsync(requestModel);
        return new ResponseContent();
    }

    [HttpDelete("remove-tag/{tagId}/{subCategoryId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveTagAsync(int tagId, Guid subCategoryId)
    {
        await _categoryService.RemoveTagAsync(tagId, subCategoryId);
        return new ResponseContent();
    }
}
