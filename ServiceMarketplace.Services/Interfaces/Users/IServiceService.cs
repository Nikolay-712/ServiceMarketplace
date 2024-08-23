using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request.Filters;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IServiceService
{
    Task<PaginationResponseModel<ServiceResponseModel>> GetAllAsync(ServiceFilter serviceFilter);
    
    Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid serviceId, RatingFilter ratingFilter);
}
