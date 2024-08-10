using ServiceMarketplace.Models.Request.Filters;
using System.Threading.Tasks;
using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Services.Interfaces.Users;

public interface IServiceService
{
    Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid serviceId, RatingFilter ratingFilter);
}
