using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Controllers.Owner;

//[Authorize(Roles = "Owner")]
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
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.CreateAsync(ownerId, requestModel);

        return new ResponseContent();
    }

    [HttpPut("update/{serviceId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateAsync([FromRoute] Guid serviceId, UpdateServiceRequestModel requestModel)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.UpdateAsync(serviceId, ownerId, requestModel);

        return new ResponseContent();
    }

    [HttpGet("all")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<ServiceResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<ServiceResponseModel>>> GetAllAsync()
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        IReadOnlyList<ServiceResponseModel> services = await _serviceService.GetAllAsync(ownerId);

        return new ResponseContent<IReadOnlyList<ServiceResponseModel>>
        {
            Result = services
        };
    }

    [HttpGet("details/{serviceId}")]
    [ProducesResponseType<ResponseContent<ServiceDetailsResponseModel>>(200)]
    public async Task<ResponseContent<ServiceDetailsResponseModel>> GetDetailsAsync(Guid serviceId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        ServiceDetailsResponseModel serviceDetails = await _serviceService.GetDetailsAsync(ownerId, serviceId);

        return new ResponseContent<ServiceDetailsResponseModel>
        {
            Result = serviceDetails
        };
    }

    [HttpPatch("change-category/{serviceId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> ChangeCategoryAsync([FromRoute] Guid serviceId, ChangeCategoryRequestModel requestModel)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.ChangeCategoryAsync(serviceId, ownerId, requestModel);

        return new ResponseContent();
    }

    [HttpPatch("add-tag/{serviceId}/{tagId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> AddTagAsync([FromRoute] Guid serviceId, [FromRoute] int tagId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.AddTagAsync(serviceId, ownerId, tagId);

        return new ResponseContent();
    }

    [HttpDelete("remove-tag/{serviceId}/{tagId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveTagAsync([FromRoute] Guid serviceId, [FromRoute] int tagId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.RemoveTagAsync(serviceId, ownerId, tagId);

        return new ResponseContent();
    }

    [HttpDelete("remove-city/{serviceId}/{cityId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveCityAsync([FromRoute] Guid serviceId, [FromRoute] Guid cityId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");
        await _serviceService.RemoveCityAsync(serviceId, ownerId, cityId);

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
