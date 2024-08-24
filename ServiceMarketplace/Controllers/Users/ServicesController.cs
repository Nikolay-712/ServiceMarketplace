using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models.Response.Services;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    [ProducesResponseType<ResponseContent<PaginationResponseModel<ServiceResponseModel>>>(200)]
    public async Task<ResponseContent<PaginationResponseModel<ServiceResponseModel>>> GetAllAsync([FromQuery] ServiceFilter serviceFilter)
    {
        PaginationResponseModel<ServiceResponseModel> services = await _serviceService.GetAllAsync(serviceFilter);
        return new ResponseContent<PaginationResponseModel<ServiceResponseModel>>
        {
            Result = services
        };
    }

    [HttpGet("{serviceId}")]
    [ProducesResponseType<ResponseContent<ServiceDetailsResponseModel>>(200)]
    public async Task<ResponseContent<ServiceDetailsResponseModel>> GetDetailsAsync([FromRoute] Guid serviceId, [FromQuery] RatingFilter ratingFilter)
    {
        ServiceDetailsResponseModel service = await _serviceService.GetDetailsAsync(serviceId, ratingFilter);
        return new ResponseContent<ServiceDetailsResponseModel>
        {
            Result = service,
        };
    }

    [HttpGet("offered-at")]
    [ProducesResponseType<IReadOnlyList<OfferedAtResponseModel>>(200)]
    public async Task<ResponseContent<IReadOnlyList<OfferedAtResponseModel>>> GetOfferedAtAsync()
    {
        IReadOnlyList<OfferedAtResponseModel> offered = await _serviceService.GetOfferedAtAsync();
        return new ResponseContent<IReadOnlyList<OfferedAtResponseModel>>
        {
            Result = offered,
        };
    }
}
