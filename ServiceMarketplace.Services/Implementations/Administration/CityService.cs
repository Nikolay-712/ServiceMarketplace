using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Services.Interfaces.Administration;

namespace ServiceMarketplace.Services.Implementations.Administration;

public class CityService : ICityService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<CityService> _logger;

    public CityService(ApplicationContext applicationContext, ILogger<CityService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task CreateAsync(ManageCityRequestModel requestModel)
    {
        bool existsCity = await _applicationContext.Cities
            .AnyAsync(x => x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn);

        if (existsCity)
        {
            _logger.LogError("A city with this name has already been added {CityNameBg}/{CityNameEn}",
                requestModel.NameBg,
                requestModel.NameEn);
            throw new ExistsCityException(Messages.ExistsCityName);
        }

        City city = new()
        {
            NameBg = requestModel.NameBg,
            NameEn = requestModel.NameEn,
        };

        _applicationContext.Cities.Add(city);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added a city with ID {CityId}", city.Id);
    }

    public async Task<PaginationResponseModel<CityResponseModel>> GetAllAsync(CityFilter cityFilter)
    {
        IQueryable<City> citiesQuery = _applicationContext.Cities;
        if (cityFilter.SearchTerm is not null)
        {
            citiesQuery = citiesQuery
                .Where(x => x.NameBg.Contains(cityFilter.SearchTerm) || x.NameEn.Contains(cityFilter.SearchTerm));
        }

        citiesQuery = citiesQuery.OrderBy(x => x.NameBg);

        int totalCount = await citiesQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / cityFilter.ItemsPerPage);

        citiesQuery = citiesQuery
            .Skip(cityFilter.SkipCount)
            .Take(cityFilter.ItemsPerPage);

        IReadOnlyList<CityResponseModel> cities = await citiesQuery
            .Select(x => new CityResponseModel(
                x.Id,
                x.NameBg,
                x.NameEn))
            .ToListAsync();

        return new PaginationResponseModel<CityResponseModel>
        {
            Items = cities,
            TotalItems = totalCount,
            PageNumber = cityFilter.PageNumber,
            ItemsPerPage = cityFilter.ItemsPerPage,
            PagesCount = pagesCount,
        };
    }

    public async Task<CityResponseModel> GetByIdAsync(Guid Id)
    {
        City? city = await _applicationContext.Cities.FirstOrDefaultAsync(x => x.Id == Id);
        if (city is null)
        {
            _logger.LogError("No city exists with this ID {CategoryId}", Id);
            throw new NotFoundEntityException(Messages.NotFoundCity);
        }

        CityResponseModel cityResponse = new(
            city.Id,
            city.NameBg,
            city.NameEn);

        return cityResponse;
    }

    public async Task UpdateAsync(Guid cityId, ManageCityRequestModel requestModel)
    {
        City? city = await _applicationContext.Cities.FirstOrDefaultAsync(x => x.Id == cityId);
        if (city is null)
        {
            _logger.LogError("No city exists with this ID {CategoryId}", cityId);
            throw new NotFoundEntityException(Messages.NotFoundCity);
        }

        bool existsCityName = await _applicationContext
            .Cities.AnyAsync(x => (x.NameBg == requestModel.NameBg || x.NameEn == requestModel.NameEn) && x.Id != cityId);

        if (existsCityName)
        {
            _logger.LogError("A city with this name has already been added {CityNameBg}/{CityNameEn}",
                requestModel.NameBg,
                requestModel.NameEn);
            throw new ExistsCityException(Messages.ExistsCityName);
        }

        city.NameBg = requestModel.NameBg;
        city.NameEn = requestModel.NameEn;

        await _applicationContext.SaveChangesAsync();
        _logger.LogInformation("Successfully updated a city with ID {CityId}", cityId);
    }
}
