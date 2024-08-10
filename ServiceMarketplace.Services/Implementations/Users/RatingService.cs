using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Users;

using static ServiceMarketplace.Models.Response.RatingResponseModels;

namespace ServiceMarketplace.Services.Implementations.Users;

public class RatingService : IRatingService
{
    private readonly ApplicationContext _applicationContext;
    private readonly IServiceService _serviceService;
    private readonly ILogger<RatingService> _logger;

    public RatingService(ApplicationContext applicationContext, IServiceService serviceService, ILogger<RatingService> logger)
    {
        _applicationContext = applicationContext;
        _serviceService = serviceService;
        _logger = logger;
    }

    public async Task AddOrUpdateRatingAsync(Guid userId, AddRatingRequestModel requestModel)
    {
        bool existsService = await _applicationContext.Services.AnyAsync(x => x.Id == requestModel.ServiceId);
        if (!existsService)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", requestModel.ServiceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        if (requestModel.Value < 1 || requestModel.Value > 10)
        {
            _logger.LogError("The rating value is not an invalid value: {RatingValue}", requestModel.Value);
            throw new InvalidRatingValueException(Messages.InvalidRatingValue);
        };

        Rating? existsUserRating = await _applicationContext.Ratings
            .FirstOrDefaultAsync(x => (x.ServiceId == requestModel.ServiceId && x.UserId == userId));

        if (existsUserRating is null)
        {
            Rating rating = new()
            {
                Value = requestModel.Value,
                Comment = requestModel.Comment,
                ServiceId = requestModel.ServiceId,
                UserId = userId,
            };

            _applicationContext.Ratings.Add(rating);
        }

        if (existsUserRating is not null)
        {
            existsUserRating.Value = requestModel.Value;
            existsUserRating.Comment = requestModel.Comment;
            existsUserRating.ModifiedOn = DateTime.UtcNow;
        }

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<UserVoteResponseModel?> GetUserVoteForServiceAsync(Guid userId, Guid serviceId)
    {
        Rating? rating = await _applicationContext.Ratings
            .Include(x => x.OwnerComment)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ServiceId == serviceId);

        if (rating is null)
        {
            return null;
        }

        UserVoteResponseModel userVote = rating.ToUserVoteResponseModel();
        return userVote;
    }
}
