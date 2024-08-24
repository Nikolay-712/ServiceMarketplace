using ServiceMarketplace.Models.Response.Cities;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface ICityService
{
    Task<IReadOnlyList<CityResponseModel>> GetAllCitiesAsync();
}
