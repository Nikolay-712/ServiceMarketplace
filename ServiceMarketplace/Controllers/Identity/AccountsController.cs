using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Identity;

using static ServiceMarketplace.Models.Response.AccountResponseModels;

namespace ServiceMarketplace.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("registration")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task RegistrationAsync(RegistrationRequestModel requestModel)
    {
        await _accountService.RegistrationAsync(requestModel);
    }

    [HttpPost("login")]
    [ProducesResponseType<ResponseContent<LoginResponseModel>>(200)]
    public async Task<ResponseContent<LoginResponseModel>> LoginAsync(LoginRequestModel requestModel)
    {
       LoginResponseModel loginResult = await _accountService.LoginAsync(requestModel);
        return new ResponseContent<LoginResponseModel>
        {
            Result = loginResult
        };
    }
}
