using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class CityService : ICityService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<CityService> _logger;

    public CityService(ApplicationContext applicationContext, ILogger<CityService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task ValidateSelectedCityAsync(Guid cityId)
    {
        bool isValidCity = await _applicationContext.Cities.AnyAsync(x => x.Id == cityId);
        if (!isValidCity)
        {
            _logger.LogError("No city exists with this ID {CityId}", cityId);
            throw new NotFoundEntityException(Messages.NotFoundCity);
        }
    }

    public async Task<IReadOnlyList<CityResponseModel>> GetAllCitiesAsync()
    {
        IReadOnlyList<CityResponseModel> cities = await _applicationContext.Cities
            .Select(x => x.ToCityResponseModel())
            .ToListAsync();

        return cities;
    }
}
