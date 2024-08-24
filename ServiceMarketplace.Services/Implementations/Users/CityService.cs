using ServiceMarketplace.Data;
using ServiceMarketplace.Models.Response.Cities;
using ServiceMarketplace.Services.Interfaces.Users;
using ServiceMarketplace.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ServiceMarketplace.Services.Implementations.Users;

public class CityService : ICityService
{
    private readonly ApplicationContext _applicationContext;

    public CityService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IReadOnlyList<CityResponseModel>> GetAllCitiesAsync()
    {
        IReadOnlyList<CityResponseModel> cities = await _applicationContext.Cities
            .Select(x => x.ToCityResponseModel())
            .ToListAsync();

        return cities;
    }
}
