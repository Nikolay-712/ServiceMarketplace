using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Implementations.Owner;
using ServiceMarketplace.Services.Interfaces.Owner;

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
}
