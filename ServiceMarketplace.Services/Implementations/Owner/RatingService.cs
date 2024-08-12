using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services.Interfaces.Owner;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Common.Exceptions.ServerExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;

using static ServiceMarketplace.Models.Response.RatingResponseModels;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class RatingService : IRatingService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<RatingService> _logger;

    public RatingService(ApplicationContext applicationContext, ILogger<RatingService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task<PaginationResponseModel<UserVoteResponseModel>> GetServiceRatingAsync(Guid serviceId, RatingFilter ratingFilter)
    {
        IQueryable<Rating> ratingsQuery = _applicationContext.Ratings
            .Include(x => x.OwnerComment)
            .Include(x => x.User)
            .Where(x => x.ServiceId == serviceId);

        ratingsQuery = ratingFilter.OrderParameters switch
        {
            RatingOrderParameters.Newest => ratingsQuery.OrderBy(x => x.CreatedOn).ThenBy(x => x.ModifiedOn),
            RatingOrderParameters.Oldest => ratingsQuery.OrderByDescending(x => x.CreatedOn).ThenByDescending(x => x.ModifiedOn),
            RatingOrderParameters.Highest_Score => ratingsQuery.OrderBy(x => x.Value).ThenBy(x => x.CreatedOn),
            RatingOrderParameters.Lowest_Score => ratingsQuery.OrderByDescending(x => x.Value).ThenBy(x => x.CreatedOn),
            _ => ratingsQuery.OrderBy(x => x.CreatedOn).ThenBy(x => x.ModifiedOn),
        };

        int totalCount = await ratingsQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / ratingFilter.ItemsPerPage);

        ratingsQuery = ratingsQuery
            .Skip(ratingFilter.SkipCount)
            .Take(ratingFilter.ItemsPerPage);

        IReadOnlyList<UserVoteResponseModel> userVotes = await ratingsQuery
              .Select(x => x.ToUserVoteResponseModel())
              .ToListAsync();

        return new PaginationResponseModel<UserVoteResponseModel>
        {
            Items = userVotes,
            TotalItems = totalCount,
            PageNumber = ratingFilter.PageNumber,
            ItemsPerPage = ratingFilter.ItemsPerPage,
            PagesCount = pagesCount,
        };
    }

    public async Task<RatingCalculationResponseModel> CalculateServiceRatingAsync(Guid serviceId)
    {
        IQueryable<Rating> ratingsQuery = _applicationContext.Ratings.Where(x => x.ServiceId == serviceId);

        bool existsVotes = await ratingsQuery.AnyAsync();
        if (!existsVotes)
        {
            return new(VotesCount: 0, AverageRating: 0);
        }

        int ratingsCount = await ratingsQuery.CountAsync();
        double averageRating = ratingsQuery.Select(x => x.Value).Average();

        return new(ratingsCount, averageRating);
    }

    public RatingResponseModel CreateRatingResponse(PaginationResponseModel<UserVoteResponseModel> userVotes, RatingCalculationResponseModel calculation)
    {
        return new(userVotes, calculation.VotesCount, calculation.AverageRating);

    }

    public async Task SendOwnerCommentAsync(Guid ownerId, SendOwnerCommentRequestModel requestModel)
    {
        Rating? rating = await _applicationContext.Ratings
             .Include(x => x.OwnerComment)
             .FirstOrDefaultAsync(x => x.Id == requestModel.RatingId);

        if (rating is null)
        {
            _logger.LogError("No rating exists with this ID {RatingId}", requestModel.RatingId);
            throw new NotFoundEntityException(Messages.NotFoundRating);
        }

        bool isServiceOwner = await _applicationContext.Services.AnyAsync(x => x.Id == rating.ServiceId && x.OwnerId == ownerId);
        if (!isServiceOwner)
        {
            _logger.LogError("A user with an ID {UserId} has no rights to a service with an ID {ServiceId}", ownerId, rating.ServiceId);
            throw new RightDeniedException(Messages.GeneralErrorMessage);
        }

        if (rating.OwnerComment is not null)
        {
            rating.OwnerComment.Comment = requestModel.Comment;
            rating.OwnerComment.ModifiedOn = DateTime.UtcNow;

            _logger.LogInformation("Successfully update comment from owner with ID {OwnerCommentId}", rating.OwnerComment.Id);
        }
        else
        {
            OwnerComment ownerComment = new()
            {
                Comment = requestModel.Comment,
                RatingId = requestModel.RatingId,
                OwnerId = ownerId,
            };

            _applicationContext.OwnerComments.Add(ownerComment);
            _logger.LogInformation("You have successfully added a comment from the owner to rating with ID {RatingId}", requestModel.RatingId);
        }

        await _applicationContext.SaveChangesAsync();
    }

    public async Task RemoveOwnerCommentAsync(Guid ownerId, int commentId)
    {
        OwnerComment? ownerComment = await _applicationContext.OwnerComments.FirstOrDefaultAsync(x => x.Id == commentId && x.OwnerId == ownerId);
        if (ownerComment is null)
        {
            _logger.LogError("No owner comment exists with this ID {CommentId}", commentId);
            throw new NotFoundEntityException(Messages.NotFoundOwnerComment);
        }

        _applicationContext.OwnerComments.Remove(ownerComment);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("You have successfully remove a comment with ID {CommentId}", commentId);
    }
}
