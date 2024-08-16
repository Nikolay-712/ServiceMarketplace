using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Owner;

[Authorize(Roles = OwnerRoleName)]
[Route("api/owner/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    [ProducesResponseType<ResponseContent<IReadOnlyList<CityResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<CityResponseModel>>> GetAllCitiesAsync()
    {
        IReadOnlyList<CityResponseModel> cities = await _cityService.GetAllCitiesAsync();
        return new ResponseContent<IReadOnlyList<CityResponseModel>>
        {
            Result = cities
        };
    }
}
