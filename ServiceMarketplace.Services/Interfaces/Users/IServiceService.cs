using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request.Filters;
using ServiceMarketplace.Models.Response.Services;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IServiceService
{
    Task<PaginationResponseModel<ServiceResponseModel>> GetAllAsync(ServiceFilter serviceFilter);
    
    Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid serviceId, RatingFilter ratingFilter);
}
