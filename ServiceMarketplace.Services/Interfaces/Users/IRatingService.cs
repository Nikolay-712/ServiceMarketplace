using ServiceMarketplace.Models.Request;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IRatingService
{
    Task AddOrUpdateRatingAsync(Guid userId, AddRatingRequestModel requestModel);
}
