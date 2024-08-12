namespace ServiceMarketplace.Models.Response;

public static class AccountResponseModels
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public bool IsConfirmedEmail { get; set; }
        public bool RememberMe { get; set; }
    }

}
