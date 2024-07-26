using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface ICityService
{
    Task ValidateSelectedCityAsync(Guid cityId);

    Task<IReadOnlyList<CityResponseModel>> GetAllCitiesAsync();
}
