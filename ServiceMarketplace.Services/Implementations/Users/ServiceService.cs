using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Services.Interfaces.Users;

using static ServiceMarketplace.Models.Response.RatingResponseModels;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;

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
}
