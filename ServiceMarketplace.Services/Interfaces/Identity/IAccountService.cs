using ServiceMarketplace.Models.Request;
using static ServiceMarketplace.Models.Response.AccountResponseModels;

namespace ServiceMarketplace.Services.Interfaces.Identity;

public interface IAccountService
{
    Task<bool> RegistrationAsync(RegistrationRequestModel requestModel);

    Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel);
}
