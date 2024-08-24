using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models.Response.Ratings;
using ServiceMarketplace.Models.Response.Services;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Services.Implementations.Users;

public class ServiceService : IServiceService
{
    private readonly ApplicationContext _applicationContext;
    private readonly IRatingService _ratingService;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(ApplicationContext applicationContext, IRatingService ratingService, ILogger<ServiceService> logger)
    {
        _applicationContext = applicationContext;
        _ratingService = ratingService;
        _logger = logger;
    }

    public async Task<PaginationResponseModel<ServiceResponseModel>> GetAllAsync(ServiceFilter serviceFilter)
    {
        IQueryable<Service> servicesQuery = _applicationContext.Services.Include(x => x.Ratings);
        servicesQuery = ApplyServiceFilters(servicesQuery, serviceFilter);

        int totalCount = await servicesQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / serviceFilter.ItemsPerPage);

        servicesQuery = servicesQuery
            .Skip(serviceFilter.SkipCount)
            .Take(serviceFilter.ItemsPerPage);


        IReadOnlyList<ServiceResponseModel> services = await servicesQuery
            .Select(x => x.ToServiceResponseModel(new RatingCalculationResponseModel(
                x.Ratings.Count(),
                x.Ratings.Any() ? x.Ratings.Average(r => r.Value) : 0)))
            .ToListAsync();

        return new PaginationResponseModel<ServiceResponseModel>
        {
            Items = services,
            TotalItems = totalCount,
            PageNumber = serviceFilter.PageNumber,
            ItemsPerPage = serviceFilter.ItemsPerPage,
            PagesCount = pagesCount,
        };
    }

    public async Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid serviceId, RatingFilter ratingFilter)
    {
        Service? service = await _applicationContext.Services
            .Include(x => x.SubCategory)
            .Include(x => x.OfferedAt)
            .Include(x => x.Cities).ThenInclude(x => x.City)
            .Include(x => x.SelectedTags).ThenInclude(x => x.Tag)
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == serviceId);

        if (service is null)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        PaginationResponseModel<UserVoteResponseModel> userVotes = await _ratingService.GetServiceRatingAsync(serviceId, ratingFilter);
        RatingCalculationResponseModel calculation = await _ratingService.CalculateServiceRatingAsync(serviceId);

        RatingResponseModel ratingResponse = new(userVotes, calculation.VotesCount, calculation.AverageRating);

        ServiceDetailsResponseModel serviceDetails = service.ToServiceDetailsResponseModel(ratingResponse);

        return serviceDetails;
    }

    private IQueryable<Service> ApplyServiceFilters(IQueryable<Service> servicesQuery, ServiceFilter serviceFilter)
    {
        if (!string.IsNullOrEmpty(serviceFilter.SearchTerm))
        {
            servicesQuery = servicesQuery.Where(x => x.NameBg.Contains(serviceFilter.SearchTerm) || x.NameEn.Contains(serviceFilter.SearchTerm));
        }

        if (serviceFilter.CategoryId != Guid.Empty)
        {
            servicesQuery = servicesQuery.Where(x => x.SubCategory.CategoryId == serviceFilter.CategoryId);
        }

        if (serviceFilter.SubCategoryId != Guid.Empty)
        {
            servicesQuery = servicesQuery.Where(x => x.SubCategoryId == serviceFilter.SubCategoryId);
        }

        if (serviceFilter.OfferedAtId is not null)
        {
            servicesQuery = servicesQuery.Where(x => x.OfferedAtId == serviceFilter.OfferedAtId);
        }

        if (serviceFilter.CityId != Guid.Empty)
        {
            servicesQuery = servicesQuery.Where(x => x.Cities.Select(x => x.CityId).Contains(serviceFilter.CityId));
        }

        if (serviceFilter.TagsId is not null)
        {
            servicesQuery = servicesQuery.Where(x => x.SelectedTags.Any(tag => serviceFilter.TagsId.Contains(tag.TagId)));
        }

        servicesQuery = serviceFilter.OrderParameters switch
        {
            ServiceOrderParameters.Date => servicesQuery.OrderBy(x => x.CreatedOn),
            ServiceOrderParameters.Name => servicesQuery.OrderBy(x => x.NameBg).ThenBy(x => x.NameEn),
            ServiceOrderParameters.Lowest_Rating => servicesQuery.OrderBy(x => x.Ratings.Any() ? x.Ratings.Average(r => r.Value) : 0),
            ServiceOrderParameters.Highest_Rating => servicesQuery.OrderByDescending(x => x.Ratings.Any() ? x.Ratings.Average(r => r.Value) : 0),

            _ => servicesQuery.OrderBy(x => x.Ratings.Select(x => x.Value).Average())
        };

        return servicesQuery;
    }
}
