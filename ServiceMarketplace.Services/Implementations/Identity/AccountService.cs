using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Data;
using ServiceMarketplace.Services.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Exceptions.ServerExceptions;
using ServiceMarketplace.Models.Response.Identity;

namespace ServiceMarketplace.Services.Implementations.Identity;

public class AccountService : IAccountService
{
    private readonly ApplicationContext _applicationContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
       ApplicationContext applicationContext,
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       ITokenManager jwtTokenManager,
       ILogger<AccountService> logger)
    {
        _applicationContext = applicationContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenManager = jwtTokenManager;
        _logger = logger;
    }

    public async Task<bool> RegistrationAsync(RegistrationRequestModel requestModel)
    {
        bool existsEmail = await _applicationContext.Users.AnyAsync(x => x.Email! == requestModel.Email);
        if (existsEmail)
        {
            _logger.LogError("Email already exists");
            throw new EmailAlreadyExistsException(Messages.EmailAlreadyExists);
        }

        ApplicationUser user = new()
        {
            //Change in production
            EmailConfirmed = true,
            UserName = requestModel.Email,
            Email = requestModel.Email,
            FullName = requestModel.FullName,
        };

        IdentityResult identityResult = await _userManager.CreateAsync(user, requestModel.Password);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new NotSuccessfulIdentityOperationException(Messages.GeneralErrorMessage);
        }

        //Change in production
        //Uri confirmationUri = await GenerateEmailConfirmationUri(user);
        //await _emailSenderService.SendEmailConfirmationAsync(user.Email, confirmationUri.AbsoluteUri);

        _logger.LogInformation("Succeeded registration with email address: {email}", requestModel.Email);
        return identityResult.Succeeded;
    }

    public async Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(requestModel.Email);
        if (user is null)
        {
            _logger.LogError("Not found user with {email}", requestModel.Email);
            throw new InvalidCredentialsException(Messages.InvalidCredentials);
        }

        LoginResponseModel loginResponse = new()
        {
            AccessToken = string.Empty,
            IsConfirmedEmail = false,
            RememberMe = false
        };

        if (!user.EmailConfirmed)
        {
            return loginResponse;
        }

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, requestModel.Password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
        {
            _logger.LogError("Invalid authentication attempt: Email {email}", requestModel.Email);
            throw new InvalidCredentialsException(Messages.InvalidCredentials);
        }

        string token = await _tokenManager.GenerateJwtTokenAsync(user);

        loginResponse.AccessToken = token;
        loginResponse.IsConfirmedEmail = true;
        loginResponse.RememberMe = requestModel.RememberMe;

        return loginResponse;
    }

    private async Task<Uri> GenerateEmailConfirmationUri(ApplicationUser user)
    {
        string token = await _tokenManager.GenerateConfirmEmailTokenAsync(user);
        string baseUrl = "https://localhost:7247";

        Uri confirmationUrl = new Uri($@"{baseUrl}/account/confirm-email?identifier={user.Id}&token={token}", new UriCreationOptions());
        return confirmationUrl;
    }
}
