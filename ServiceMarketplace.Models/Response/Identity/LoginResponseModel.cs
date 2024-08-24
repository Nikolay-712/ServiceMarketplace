namespace ServiceMarketplace.Models.Response.Identity;

public class LoginResponseModel
{
    public string AccessToken { get; set; }
    public bool IsConfirmedEmail { get; set; }
    public bool RememberMe { get; set; }
}
