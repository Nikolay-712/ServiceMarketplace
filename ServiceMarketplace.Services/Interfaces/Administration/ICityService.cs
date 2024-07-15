using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Services.Interfaces.Administration;

public interface ICityService
{
    Task CreateAsync(ManageCityRequestModel requestModel);

    Task<PaginationResponseModel<CityResponseModel>> GetAllAsync(CityFilter cityFilter);

    Task<CityResponseModel> GetByIdAsync(Guid Id);

    Task UpdateAsync(Guid cityId, ManageCityRequestModel requestModel);
}
