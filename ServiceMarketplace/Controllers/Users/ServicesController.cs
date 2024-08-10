using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Services.Interfaces.Users;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;

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
}
