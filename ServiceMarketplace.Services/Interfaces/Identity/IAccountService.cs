using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Identity;

namespace ServiceMarketplace.Services.Interfaces.Identity;

public interface IAccountService
{
    Task<bool> RegistrationAsync(RegistrationRequestModel requestModel);

    Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel);
}
