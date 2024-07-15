using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Controllers.Administration;

[Route("api/administration/[controller]")]
[ApiController]
//[Authorize(Roles = "Administrator")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpPost("create")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> CreateAsync(ManageCityRequestModel requestModel)
    {
        await _cityService.CreateAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("all")]
    [ProducesResponseType<ResponseContent<PaginationResponseModel<CityResponseModel>>>(200)]
    public async Task<ResponseContent<PaginationResponseModel<CityResponseModel>>> GetAllAsync([FromQuery] CityFilter cityFilter)
    {
        PaginationResponseModel<CityResponseModel> citiesResponse = await _cityService.GetAllAsync(cityFilter);
        return new ResponseContent<PaginationResponseModel<CityResponseModel>>
        {
            Result = citiesResponse
        };
    }

    [HttpGet("{cityId}")]
    [ProducesResponseType<ResponseContent<CityResponseModel>>(200)]
    public async Task<ResponseContent<CityResponseModel>> GetByIdAsync(Guid cityId)
    {
        CityResponseModel city = await _cityService.GetByIdAsync(cityId);
        return new ResponseContent<CityResponseModel>
        {
            Result = city
        };
    }

    [HttpPut("update/{cityId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateAsync(Guid cityId, ManageCityRequestModel requestModel)
    {
        await _cityService.UpdateAsync(cityId, requestModel);
        return new ResponseContent();
    }
}
