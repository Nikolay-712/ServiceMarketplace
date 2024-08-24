using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models.Response.Cities;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Controllers.Users;

[Route("api/[controller]")]
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
