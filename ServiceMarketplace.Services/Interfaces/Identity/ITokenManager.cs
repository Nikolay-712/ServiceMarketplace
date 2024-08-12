using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Services.Interfaces.Identity;

public interface ITokenManager
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user);

    Task<string> GenerateConfirmEmailTokenAsync(ApplicationUser user);
}
