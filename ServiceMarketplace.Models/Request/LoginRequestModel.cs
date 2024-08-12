namespace ServiceMarketplace.Models.Request;

public record LoginRequestModel(
    string Email, 
    string Password, 
    bool RememberMe);


