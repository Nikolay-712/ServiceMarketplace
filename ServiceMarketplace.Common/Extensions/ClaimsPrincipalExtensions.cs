using System.Security.Claims;

namespace ServiceMarketplace.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Guid.Parse(userId);
    }
}
