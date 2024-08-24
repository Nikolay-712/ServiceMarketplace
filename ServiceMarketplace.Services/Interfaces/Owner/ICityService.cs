using ServiceMarketplace.Models.Response.Cities;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface ICityService
{
    Task ValidateSelectedCityAsync(Guid cityId);

    Task<IReadOnlyList<CityResponseModel>> GetAllCitiesAsync();
}
