using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Services.Interfaces.Owner;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Models.Response.Services;

using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Owner;

[Authorize(Roles = OwnerRoleName)]
[Route("api/owner/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpPost("create")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateAsync(CreateServiceRequestModel requestModel)
    {
        await _serviceService.CreateAsync(ClaimsPrincipalExtensions.GetUserId(this.User), requestModel);

        return new ResponseContent();
    }

    [HttpPut("update/{serviceId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateAsync([FromRoute] Guid serviceId, UpdateServiceRequestModel requestModel)
    {
        
        await _serviceService.UpdateAsync(serviceId, ClaimsPrincipalExtensions.GetUserId(this.User), requestModel);

        return new ResponseContent();
    }

    [HttpGet("all")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<ServiceResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<ServiceResponseModel>>> GetAllAsync()
    {
        IReadOnlyList<ServiceResponseModel> services = await _serviceService.GetAllAsync(ClaimsPrincipalExtensions.GetUserId(this.User));

        return new ResponseContent<IReadOnlyList<ServiceResponseModel>>
        {
            Result = services
        };
    }

    [HttpGet("details/{serviceId}")]
    [ProducesResponseType<ResponseContent<ServiceDetailsResponseModel>>(200)]
    public async Task<ResponseContent<ServiceDetailsResponseModel>> GetDetailsAsync(Guid serviceId, [FromQuery] RatingFilter ratingFilter)
    {
        ServiceDetailsResponseModel serviceDetails = await _serviceService.GetDetailsAsync(ClaimsPrincipalExtensions.GetUserId(this.User), serviceId, ratingFilter);

        return new ResponseContent<ServiceDetailsResponseModel>
        {
            Result = serviceDetails
        };
    }

    [HttpPatch("change-category/{serviceId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> ChangeCategoryAsync([FromRoute] Guid serviceId, ChangeCategoryRequestModel requestModel)
    {
        await _serviceService.ChangeCategoryAsync(serviceId, ClaimsPrincipalExtensions.GetUserId(this.User), requestModel);

        return new ResponseContent();
    }

    [HttpPatch("add-tag/{serviceId}/{tagId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> AddTagAsync([FromRoute] Guid serviceId, [FromRoute] int tagId)
    {
        await _serviceService.AddTagAsync(serviceId, ClaimsPrincipalExtensions.GetUserId(this.User), tagId);

        return new ResponseContent();
    }

    [HttpDelete("remove-tag/{serviceId}/{tagId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveTagAsync([FromRoute] Guid serviceId, [FromRoute] int tagId)
    {
        await _serviceService.RemoveTagAsync(serviceId, ClaimsPrincipalExtensions.GetUserId(this.User), tagId);

        return new ResponseContent();
    }

    [HttpDelete("remove-city/{serviceId}/{cityId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveCityAsync([FromRoute] Guid serviceId, [FromRoute] Guid cityId)
    {
        await _serviceService.RemoveCityAsync(serviceId, ClaimsPrincipalExtensions.GetUserId(this.User), cityId);

        return new ResponseContent();
    }

    [HttpGet("offered-at")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<OfferedAtResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<OfferedAtResponseModel>>> GetAllOptionsAsync()
    {
        IReadOnlyList<OfferedAtResponseModel> options = await _serviceService.GetOfferedAtOptionsAsync();
        return new ResponseContent<IReadOnlyList<OfferedAtResponseModel>>
        {
            Result = options
        };
    }
}
